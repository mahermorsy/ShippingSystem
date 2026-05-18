$(document).ready(function () {
    loadCountries();
    loadShippingTypes();
    loadShippingPackaging();
    loadPaymentMethods();
    applyValidationAttributes();

    $("#SenderCountryId").on("change", function () {
        loadCitiesByCountry("#SenderCityId", $(this).val());
    });

    $("#ReceiverCountryId").on("change", function () {
        loadCitiesByCountry("#ReceiverCityId", $(this).val());
    });

    $("#EditShipmentLink").on("click", function (event) {
        event.preventDefault();
        if (typeof window.goToShipmentStep === "function") {
            window.goToShipmentStep(0);
        }
    });

    $("#CreateShipmentBtn").on("click", submitShipment);
});

let shipmentPaymentPayload = null;
let paypalButtonsRendered = false;

function fillDropdown(selector, items, textField, placeholder, valueField = "id") {
    const ddl = $(selector);
    ddl.empty().append(`<option value="">${placeholder}</option>`);

    (items || []).forEach(item => {
        ddl.append(`<option value="${item[valueField]}">${item[textField] ?? ""}</option>`);
    });
}

function getData(res) {
    return res.data || res.Data || [];
}

function loadCountries() {
    CountriesService.getAll(function (res) {
        const data = getData(res);
        fillDropdown("#SenderCountryId", data, "countryEname", "Select country");
        fillDropdown("#ReceiverCountryId", data, "countryEname", "Select country");
    }, function () {
        AppHelper.showToast("Error loading countries", "error");
    });
}

function loadCitiesByCountry(selector, countryId) {
    if (!countryId) {
        $(selector).empty().append(`<option value="">Select city</option>`);
        return;
    }

    CitiesService.getByCountryId(countryId, function (res) {
        fillDropdown(selector, getData(res), "cityNameEn", "Select city", "cityId");
    }, function () {
        AppHelper.showToast("Error loading cities", "error");
    });
}

function loadShippingTypes() {
    ShippingTypesService.getAll(function (res) {
        fillDropdown("#ShippingTypeId", getData(res), "shippingTypeEname", "Select shipping type");
    }, function () {
        AppHelper.showToast("Error loading shipping types", "error");
    });
}

function loadShippingPackaging() {
    ShippingPackagingService.getAll(function (res) {
        fillDropdown("#ShippingPackagingId", getData(res), "shippingPackagingEname", "Select packaging");
    }, function () {
        AppHelper.showToast("Error loading packaging", "error");
    });
}

function loadPaymentMethods() {
    PaymentMethodsService.getAll(function (res) {
        fillDropdown("#PaymentMethodId", getData(res), "methodEname", "Select payment method");
    }, function () {
        AppHelper.showToast("Error loading payment methods", "error");
    });
}

function getGuidOrNull(selector) {
    const value = $(selector).val();
    return value ? value : null;
}

function getNumberValue(selector) {
    return Number($(selector).val() || 0);
}

function getSelectedText(selector) {
    return $(selector).find("option:selected").text() || "";
}

function hasValue(value) {
    return value !== null && value !== undefined && String(value).trim() !== "";
}

function applyValidationAttributes() {
    const rules = {
        "#SenderCountryId": { name: "Sender.CountryId", required: true },
        "#SenderCityId": { name: "Sender.CityId", required: true },
        "#SenderName": { name: "Sender.SenderName", required: true, minlength: 3, maxlength: 100 },
        "#SenderContact": { name: "Sender.Contact", maxlength: 100 },
        "#SenderEmail": { name: "Sender.Email", required: true },
        "#SenderPhone": { name: "Sender.Phone", required: true, minlength: 7, maxlength: 20 },
        "#SenderAddress": { name: "Sender.Address", required: true, minlength: 5, maxlength: 200 },
        "#SenderAddressDetails": { name: "Sender.AddressDetails", maxlength: 200 },
        "#SenderOtherAddress": { name: "Sender.OtherAddress", maxlength: 200 },
        "#SenderPostalCode": { name: "Sender.PostalCode", maxlength: 20 },
        "#ReceiverCountryId": { name: "Receiver.CountryId", required: true },
        "#ReceiverCityId": { name: "Receiver.CityId", required: true },
        "#ReceiverName": { name: "Receiver.ReceiverName", required: true, minlength: 3, maxlength: 100 },
        "#ReceiverEmail": { name: "Receiver.Email", required: true },
        "#ReceiverPhone": { name: "Receiver.Phone", required: true, minlength: 7, maxlength: 20 },
        "#ReceiverAddress": { name: "Receiver.Address", required: true, minlength: 5, maxlength: 200 },
        "#ReceiverAddressDetails": { name: "Receiver.AddressDetails", maxlength: 200 },
        "#ReceiverOtherAddress": { name: "Receiver.OtherAddress", maxlength: 200 },
        "#ReceiverPostalCode": { name: "Receiver.PostalCode", maxlength: 20 },
        "#ShippingPackagingId": { name: "ShippingPackagingId" },
        "#Weight": { name: "Weight", required: true, min: 0.01, max: 1000 },
        "#Length": { name: "Length", required: true, min: 0.01, max: 500 },
        "#Width": { name: "Width", required: true, min: 0.01, max: 500 },
        "#Height": { name: "Height", required: true, min: 0.01, max: 500 },
        "#PackageValue": { name: "PackageValue", required: true, min: 0.01, max: 500000 },
        "#ShippingDate": { name: "ShippingDate", required: true },
        "#DeliveryDate": { name: "DeliveryDate", required: true },
        "#ShippingTypeId": { name: "ShippingTypeId", required: true },
        "#PaymentMethodId": { name: "PaymentMethodId" }
    };

    Object.entries(rules).forEach(([selector, attrs]) => {
        const element = $(selector);

        if (!element.length) {
            return;
        }

        Object.entries(attrs).forEach(([key, value]) => {
            if (typeof value === "boolean") {
                if (value) {
                    element.attr(key, key);
                } else {
                    element.removeAttr(key);
                }
                return;
            }

            element.attr(key, value);
        });
    });
}

function validateShipmentPayload(payload) {
    const requiredChecks = [
        hasValue(payload?.shippingDate),
        hasValue(payload?.deliveryDate),
        hasValue(payload?.shippingTypeId),
        payload?.weight > 0 && payload?.weight <= 1000,
        payload?.length > 0 && payload?.length <= 500,
        payload?.width > 0 && payload?.width <= 500,
        payload?.height > 0 && payload?.height <= 500,
        payload?.packageValue > 0 && payload?.packageValue <= 500000,
        hasValue(payload?.sender?.senderName),
        hasValue(payload?.sender?.email),
        hasValue(payload?.sender?.phone),
        hasValue(payload?.sender?.address),
        hasValue(payload?.sender?.countryId),
        hasValue(payload?.sender?.cityId),
        hasValue(payload?.receiver?.receiverName),
        hasValue(payload?.receiver?.email),
        hasValue(payload?.receiver?.phone),
        hasValue(payload?.receiver?.address),
        hasValue(payload?.receiver?.countryId),
        hasValue(payload?.receiver?.cityId),
        payload?.sender?.senderName?.trim().length >= 3,
        payload?.receiver?.receiverName?.trim().length >= 3,
        payload?.sender?.address?.trim().length >= 5,
        payload?.receiver?.address?.trim().length >= 5,
        payload?.sender?.phone?.trim().length >= 7,
        payload?.receiver?.phone?.trim().length >= 7
    ];

    return requiredChecks.every(Boolean);
}

function setReviewList(selector, items) {
    const list = $(selector);
    list.empty();

    items.forEach(item => {
        list.append(`<li><strong>${item.label}:</strong> ${item.value || "-"}</li>`);
    });
}

function renderReviewSummary() {
    setReviewList("#SenderReviewList", [
        { label: "Country", value: getSelectedText("#SenderCountryId") },
        { label: "City", value: getSelectedText("#SenderCityId") },
        { label: "Name", value: $("#SenderName").val() },
        { label: "Contact", value: $("#SenderContact").val() },
        { label: "Email", value: $("#SenderEmail").val() },
        { label: "Phone", value: $("#SenderPhone").val() },
        { label: "Address", value: $("#SenderAddress").val() },
        { label: "Address Details", value: $("#SenderAddressDetails").val() },
        { label: "Other Address", value: $("#SenderOtherAddress").val() },
        { label: "Postal Code", value: $("#SenderPostalCode").val() },
        { label: "Default Address", value: $("#SenderIsDefault").is(":checked") ? "Yes" : "No" }
    ]);

    setReviewList("#ReceiverReviewList", [
        { label: "Country", value: getSelectedText("#ReceiverCountryId") },
        { label: "City", value: getSelectedText("#ReceiverCityId") },
        { label: "Name", value: $("#ReceiverName").val() },
        { label: "Email", value: $("#ReceiverEmail").val() },
        { label: "Phone", value: $("#ReceiverPhone").val() },
        { label: "Address", value: $("#ReceiverAddress").val() },
        { label: "Address Details", value: $("#ReceiverAddressDetails").val() },
        { label: "Other Address", value: $("#ReceiverOtherAddress").val() },
        { label: "Postal Code", value: $("#ReceiverPostalCode").val() },
        { label: "Default Address", value: $("#ReceiverIsDefault").is(":checked") ? "Yes" : "No" }
    ]);

    setReviewList("#ShipmentDetailsReviewList", [
        { label: "Packaging", value: getSelectedText("#ShippingPackagingId") },
        { label: "Weight", value: $("#Weight").val() ? `${$("#Weight").val()} kg` : "" },
        { label: "Length", value: $("#Length").val() },
        { label: "Width", value: $("#Width").val() },
        { label: "Height", value: $("#Height").val() },
        { label: "Declared Value", value: $("#PackageValue").val() },
        { label: "Shipping Date", value: $("#ShippingDate").val() },
        { label: "Delivery Date", value: $("#DeliveryDate").val() },
        { label: "Shipping Type", value: getSelectedText("#ShippingTypeId") },
        { label: "Payment Method", value: getSelectedText("#PaymentMethodId") }
    ]);
}

window.onShipmentStepChanged = function (nextFieldset) {
    if (nextFieldset.find("#ShipmentReviewSummary").length) {
        renderReviewSummary();
    }
};

function buildShipmentPayload() {
    return {
        shippingDate: $("#ShippingDate").val(),
        deliveryDate: $("#DeliveryDate").val(),
        shippingTypeId: $("#ShippingTypeId").val(),
        shippingPackagingId: getGuidOrNull("#ShippingPackagingId"),
        width: getNumberValue("#Width"),
        height: getNumberValue("#Height"),
        weight: getNumberValue("#Weight"),
        length: getNumberValue("#Length"),
        packageValue: getNumberValue("#PackageValue"),
        paymentMethodId: getGuidOrNull("#PaymentMethodId"),

        sender: {
            senderName: $("#SenderName").val(),
            email: $("#SenderEmail").val(),
            phone: $("#SenderPhone").val(),
            contact: $("#SenderContact").val(),
            cityId: $("#SenderCityId").val(),
            countryId: $("#SenderCountryId").val(),
            address: $("#SenderAddress").val(),
            addressDetails: $("#SenderAddressDetails").val(),
            otherAddress: $("#SenderOtherAddress").val(),
            isDefault: $("#SenderIsDefault").is(":checked"),
            postalCode: $("#SenderPostalCode").val()
        },

        receiver: {
            receiverName: $("#ReceiverName").val(),
            email: $("#ReceiverEmail").val(),
            phone: $("#ReceiverPhone").val(),
            cityId: $("#ReceiverCityId").val(),
            countryId: $("#ReceiverCountryId").val(),
            address: $("#ReceiverAddress").val(),
            addressDetails: $("#ReceiverAddressDetails").val(),
            otherAddress: $("#ReceiverOtherAddress").val(),
            isDefault: $("#ReceiverIsDefault").is(":checked"),
            postalCode: $("#ReceiverPostalCode").val()
        }
    };
}

function buildPaymentRequest(payload) {
    const shippingAmount = Number(
        (
            (payload.weight * 0.5) +
            (payload.length * 0.2) +
            (payload.width * 0.2) +
            (payload.height * 0.2)
        ).toFixed(2)
    );

    return {
        totalAmount: shippingAmount,
        cartItems: [
            {
                name: "Shipping Service",
                description: `${getSelectedText("#ShippingTypeId")} shipment`,
                price: shippingAmount,
                quantity: 1
            }
        ]
    };
}

function showPaymentPanel(payload, message) {
    shipmentPaymentPayload = payload;
    $("#CreateShipmentBtn").hide();
    $("#ShipmentPaymentPanel").show();

    if (message) {
        $("#paypal-result-message").text(message).removeClass("text-danger").addClass("text-success");
    }

    renderPayPalButtons();
}

function renderPayPalButtons() {
    if (paypalButtonsRendered) {
        return;
    }

    if (!window.paypal) {
        $("#paypal-result-message")
            .text("PayPal checkout is not loaded. Please refresh and try again.")
            .removeClass("text-success")
            .addClass("text-danger");
        return;
    }

    paypalButtonsRendered = true;

    window.paypal.Buttons({
        style: {
            shape: "rect",
            layout: "vertical",
            color: "silver",
            label: "paypal"
        },

        async createOrder() {
            const response = await fetch(`${ApiClient.baseUrl}/api/Payment/Create`, {
                method: "POST",
                headers: ApiClient.buildHeaders ? ApiClient.buildHeaders(true) : { "Content-Type": "application/json" },
                credentials: "include",
                body: JSON.stringify(buildPaymentRequest(shipmentPaymentPayload))
            });

            const orderData = await readJsonResponse(response);

            const orderId = orderData?.id || orderData?.orderID || orderData?.OrderID;

            if (typeof orderId === "string" && orderId.trim()) {
                return orderId;
            }

            if (typeof orderData === "string" && orderData.trim()) {
                return orderData;
            }

            throw new Error(extractPayPalMessage(orderData, "Could not create PayPal order."));
        },

        async onApprove(data, actions) {
            const response = await fetch(`${ApiClient.baseUrl}/api/Payment/Capture`, {
                method: "POST",
                headers: ApiClient.buildHeaders ? ApiClient.buildHeaders(true) : { "Content-Type": "application/json" },
                credentials: "include",
                body: JSON.stringify({
                    orderId: data.orderID,
                    amount: buildPaymentRequest(shipmentPaymentPayload).totalAmount
                })
            });

            const orderData = await readJsonResponse(response);
            const errorDetail = orderData?.details?.[0];

            if (errorDetail?.issue === "INSTRUMENT_DECLINED") {
                return actions.restart();
            }

            if (errorDetail) {
                throw new Error(extractPayPalMessage(orderData, "PayPal payment failed."));
            }

            const capture =
                orderData?.purchase_units?.[0]?.payments?.captures?.[0] ||
                orderData?.purchase_units?.[0]?.payments?.authorizations?.[0];

            const paymentMessage = capture
                ? `Payment ${capture.status}: ${capture.id}`
                : "Payment completed successfully.";

            $("#paypal-result-message")
                .text(paymentMessage)
                .removeClass("text-danger")
                .addClass("text-success");

            AppHelper.showSuccessAndRedirect("Shipment created and payment completed successfully.", "/");
        },

        onError(error) {
            console.error("PayPal checkout failed:", error);
            $("#paypal-result-message")
                .text(error?.message || "PayPal checkout failed.")
                .removeClass("text-success")
                .addClass("text-danger");
        }
    }).render("#paypal-button-container");
}

async function readJsonResponse(response) {
    const text = await response.text();

    if (!text) {
        if (!response.ok) {
            throw new Error(`Request failed with status ${response.status}`);
        }

        return {};
    }

    try {
        return JSON.parse(text);
    } catch (error) {
        console.error("Invalid JSON response:", text);
        throw new Error("Server returned an invalid response.");
    }
}

function extractPayPalMessage(data, fallback) {
    const detail = data?.details?.[0];
    if (detail?.description) return detail.description;
    if (data?.message) return data.message;
    if (data?.error) return data.error;
    return fallback;
}

function submitShipment() {
    const payload = buildShipmentPayload();
    const form = document.getElementById("CreateShipmentForm");

    if (!validateShipmentPayload(payload)) {
        AppHelper.showToast("Please complete all required shipment fields before submitting.", "error");
        return;
    }

    const btn = $("#CreateShipmentBtn");
    btn.prop("disabled", true).val("Submitting...");

    ApiClient.post("/api/Shipments/Create", payload, function (res) {
        const message = res.message || res.Message || "Shipment created successfully";

        btn.prop("disabled", false).val("Submit");
        showPaymentPanel(payload, message);

    }, function (xhr) {
        const serverErrors = extractServerErrors(xhr);
        const message =
            serverErrors[0] ||
            xhr.responseJSON?.message ||
            xhr.responseJSON?.Message ||
            "Error creating shipment";

        AppHelper.showToast(message, "error");
        console.error("Create shipment failed:", {
            status: xhr?.status,
            responseJSON: xhr?.responseJSON,
            responseText: xhr?.responseText,
            serverErrors
        });
        btn.prop("disabled", false).val("Submit");
    }, true);
}

function extractServerErrors(xhr) {
    const response = xhr?.responseJSON || {};
    const errors = response.errors || response.Errors;

    if (Array.isArray(errors)) {
        return errors.filter(Boolean);
    }

    if (errors && typeof errors === "object") {
        return Object.values(errors).flat().filter(Boolean);
    }

    return [];
}
