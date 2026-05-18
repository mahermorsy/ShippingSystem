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

    window.paypal.Buttons({
        style: {
            shape: "rect",
            layout: "vertical",
            color: "silver",
            label: "paypal"
        },

        async createOrder() {
            const request = buildTestPaymentRequest();
            const response = await fetch(`${ApiClient.baseUrl}/api/Payment/Create`, {
                method: "POST",
                headers: ApiClient.buildHeaders ? ApiClient.buildHeaders(false) : { "Content-Type": "application/json" },
                credentials: "include",
                body: JSON.stringify(request)
            });

            const orderData = await readPaymentJsonResponse(response);
            writePaymentDebug("Create order response", orderData);

            if (!response.ok) {
                throw new Error(extractPayPalMessage(orderData, "Could not create PayPal order."));
            }

            const orderId = orderData?.id || orderData?.orderID || orderData?.OrderID;
            if (typeof orderId === "string" && orderId.trim()) {
                setPaymentStatus(`PayPal order created: ${orderId}`, false);
                return orderId;
            }

            throw new Error(extractPayPalMessage(orderData, "PayPal order response did not include an order id."));
        },

        async onApprove(data, actions) {
            const request = buildTestPaymentRequest();
            const captureRequest = {
                orderId: data.orderID,
                amount: request.totalAmount
            };

            writePaymentDebug("Capture request", captureRequest);

            const response = await fetch(`${ApiClient.baseUrl}/api/Payment/Capture`, {
                method: "POST",
                headers: ApiClient.buildHeaders ? ApiClient.buildHeaders(false) : { "Content-Type": "application/json" },
                credentials: "include",
                body: JSON.stringify(captureRequest)
            });

            const orderData = await readPaymentJsonResponse(response);
            writePaymentDebug("Capture response", orderData);

            const errorDetail = orderData?.details?.[0] || orderData?.Details?.details?.[0];
            if (errorDetail?.issue === "INSTRUMENT_DECLINED") {
                return actions.restart();
            }

            if (!response.ok || errorDetail) {
                throw new Error(extractPayPalMessage(orderData, "PayPal payment failed."));
            }

            const capture =
                orderData?.purchase_units?.[0]?.payments?.captures?.[0] ||
                orderData?.purchase_units?.[0]?.payments?.authorizations?.[0];

            const message = capture
                ? `Payment ${capture.status}: ${capture.id}`
                : "Payment completed successfully.";

            setPaymentStatus(message, false);
        },

        onError(error) {
            console.error("PayPal checkout failed:", error);
            setPaymentStatus(error?.message || "PayPal checkout failed.", true);
        }
    }).render("#paypal-button-container");
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

async function readPaymentJsonResponse(response) {
    const text = await response.text();
    if (!text) {
        return {};
    }

    try {
        return JSON.parse(text);
    } catch (error) {
        console.error("Invalid JSON response:", text);
        return { message: text };
    }
}

function extractPayPalMessage(data, fallback) {
    const detail = data?.details?.[0] || data?.Details?.details?.[0];
    if (detail?.description) return detail.description;
    if (detail?.issue) return detail.issue;
    if (data?.Message) return data.Message;
    if (data?.message) return data.message;
    if (data?.Details?.message) return data.Details.message;
    if (data?.error) return data.error;
    return fallback;
}

function setPaymentStatus(message, isError) {
    $("#paypal-result-message")
        .text(message)
        .toggleClass("text-danger", Boolean(isError))
        .toggleClass("text-success", !isError);
}

function writePaymentDebug(label, data) {
    const debugOutput = $("#paypal-debug-output");
    const previousText = debugOutput.text();
    const nextText = `${label}\n${JSON.stringify(data, null, 2)}`;

    debugOutput
        .show()
        .text(previousText ? `${previousText}\n\n${nextText}` : nextText);
}
