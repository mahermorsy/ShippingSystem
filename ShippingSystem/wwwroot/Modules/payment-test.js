$(document).ready(function () {
    renderPaymentButtons();
});

let paymentButtonsRendered = false;

function renderPaymentButtons() {
    if (paymentButtonsRendered) {
        return;
    }

    if (!window.paypal) {
        setPaymentStatus("PayPal checkout is not loaded. Please refresh and try again.", true);
        return;
    }

    paymentButtonsRendered = true;
    clearPaymentFeedback();

    window.paypal.Buttons({
        style: {
            shape: "rect",
            layout: "vertical",
            color: "silver",
            label: "paypal"
        },

        async createOrder(data, actions) {
            const request = buildTestPaymentRequest();
            setPaymentStatus("PayPal checkout is ready.", false);
            return actions.order.create(buildPayPalOrderPayload(request));
        },

        async onApprove(data, actions) {
            const orderData = await actions.order.capture();

            const capture = getPayPalCapture(orderData);

            const message = capture
                ? `Payment ${capture.status}: ${capture.id}`
                : "Payment completed successfully.";

            setPaymentStatus(message, false);
        },

        onError(error) {
            console.error("PayPal checkout failed:", error);
            setPaymentStatus(getFriendlyPayPalError(error), true);
        }
    }).render("#paypal-button-container");
}

function buildPayPalOrderPayload(paymentRequest) {
    const items = (paymentRequest.cartItems || []).map(item => ({
        name: item.name || "Shipping Service",
        description: item.description || "Shipment payment",
        quantity: String(item.quantity || 1),
        category: "PHYSICAL_GOODS",
        unit_amount: {
            currency_code: "USD",
            value: formatPayPalAmount(item.price)
        }
    }));

    return {
        intent: "CAPTURE",
        purchase_units: [{
            amount: {
                currency_code: "USD",
                value: formatPayPalAmount(paymentRequest.totalAmount),
                breakdown: {
                    item_total: {
                        currency_code: "USD",
                        value: formatPayPalAmount(paymentRequest.totalAmount)
                    }
                }
            },
            items
        }]
    };
}

function formatPayPalAmount(value) {
    return Number(value || 0).toFixed(2);
}

function getPayPalCapture(orderData) {
    return orderData?.purchase_units?.[0]?.payments?.captures?.[0] ||
        orderData?.purchase_units?.[0]?.payments?.authorizations?.[0];
}

function buildTestPaymentRequest() {
    const amount = Number($("#PaymentAmount").val() || 0);
    const quantity = Number($("#PaymentQuantity").val() || 1);
    const price = Number(amount.toFixed(2));

    return {
        totalAmount: price * quantity,
        cartItems: [
            {
                name: $("#PaymentItemName").val() || "Shipping Service",
                description: $("#PaymentItemDescription").val() || "Temporary shipment payment",
                price,
                quantity
            }
        ]
    };
}

function setPaymentStatus(message, isError) {
    $("#paypal-result-message")
        .text(message)
        .toggleClass("text-danger", Boolean(isError))
        .toggleClass("text-success", !isError);
}

function clearPaymentFeedback() {
    $("#paypal-result-message")
        .text("")
        .removeClass("text-danger text-success");

    $("#paypal-debug-output")
        .hide()
        .text("");
}

function getFriendlyPayPalError(error) {
    const message = error?.message || "";

    if (message.toLowerCase().includes("unauthorized")) {
        return "PayPal refused this sandbox order. Please refresh the page and try again with a sandbox buyer account.";
    }

    return message || "PayPal checkout failed.";
}
