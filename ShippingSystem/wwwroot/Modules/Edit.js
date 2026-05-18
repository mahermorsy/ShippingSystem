const editShipmentState = {
    shipmentId: null,
    currentShipment: null
};

$(document).ready(function () {
    editShipmentState.shipmentId = getShipmentIdFromQuery();

    if (!editShipmentState.shipmentId) {
        AppHelper.showToast("Shipment id is missing.", "error");
        window.location.href = "/Shipments/List";
        return;
    }

    applyValidationAttributes();
    bindEvents();
    initializeEditShipment();
});

function bindEvents() {
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

    $("#UpdateShipmentBtn").on("click", submitShipmentUpdate);
}

async function initializeEditShipment() {
    try {
        setStatus("Loading shipment data...");
        disableUpdateButton(true, "Loading...");

        const [countries, shippingTypes, packaging, paymentMethods, shipment] = await Promise.all([
            getCountriesAsync(),
            getShippingTypesAsync(),
            getShippingPackagingAsync(),
            getPaymentMethodsAsync(),
            getShipmentByIdAsync(editShipmentState.shipmentId)
        ]);

        fillDropdown("#SenderCountryId", countries, "countryEname", "Select country");
        fillDropdown("#ReceiverCountryId", countries, "countryEname", "Select country");
        fillDropdown("#ShippingTypeId", shippingTypes, "shippingTypeEname", "Select shipping type");
        fillDropdown("#ShippingPackagingId", packaging, "shippingPackagingEname", "Select packaging");
        fillDropdown("#PaymentMethodId", paymentMethods, "methodEname", "Select payment method");

        await populateShipmentForm(shipment);

        setStatus("Shipment data loaded successfully.", true);
        disableUpdateButton(false, "Update Shipment");
    } catch (error) {
        console.log(error);
        setStatus("Error loading shipment data.", false, true);
        AppHelper.showToast("Error loading shipment data", "error");
        disableUpdateButton(true, "Update Shipment");
    }
}
/////////////////////////////////////////////////////////////////// النماذج الخاصه بملئ الحقول وجلب البيانات    ////////////////////////////////////////////////
function fillDropdown(selector, items, textField, placeholder, valueField = "id") {
    const ddl = $(selector);
    ddl.empty().append(`<option value="">${placeholder}</option>`);

    (items || []).forEach(item => {
        ddl.append(`<option value="${item[valueField]}">${item[textField] ?? ""}</option>`);
    });
}

function getData(res) {
    return res?.data || res?.Data || [];
}

function getShipmentData(res) {
    return res?.data || res?.Data || res;
}

function loadCitiesByCountry(selector, countryId, selectedCityId = null) {
    if (!countryId) {
        $(selector).empty().append('<option value="">Select city</option>');
        return Promise.resolve();
    }

    return new Promise((resolve, reject) => {
        CitiesService.getByCountryId(countryId, function (res) {
            fillDropdown(selector, getData(res), "cityNameEn", "Select city", "cityId");

            if (selectedCityId) {
                $(selector).val(selectedCityId);
            }

            resolve();
        }, function (error) {
            AppHelper.showToast("Error loading cities", "error");
            reject(error);
        });
    });
}

function getCountriesAsync() {
    return new Promise((resolve, reject) => {
        CountriesService.getAll(function (res) {
            resolve(getData(res));
        }, reject);
    });
}

function getShippingTypesAsync() {
    return new Promise((resolve, reject) => {
        ShippingTypesService.getAll(function (res) {
            resolve(getData(res));
        }, reject);
    });
}

function getShippingPackagingAsync() {
    return new Promise((resolve, reject) => { ShippingPackagingService.getAll(function (res) {resolve(getData(res));}, reject);
    });
}

function getPaymentMethodsAsync() {
    return new Promise((resolve, reject) => {
        PaymentMethodsService.getAll(function (res) {
            resolve(getData(res));
        }, reject);
    });
}

function getShipmentByIdAsync(id) {
    return new Promise((resolve, reject) => {
        ShipmentsService.getById(id, function (res) {
            resolve(getShipmentData(res));
        }, reject);
    });
}
///////////////////////////////////////////////////////////////////  نشغل الشحنة في النموذج   ////////////////////////////////////////////////////////////////////  

async function populateShipmentForm(shipment) {
    editShipmentState.currentShipment = shipment;

    $("#SenderCountryId").val(shipment.sender?.countryId || "");
    $("#ReceiverCountryId").val(shipment.receiver?.countryId || "");

    await Promise.all([
        loadCitiesByCountry("#SenderCityId", shipment.sender?.countryId, shipment.sender?.cityId),
        loadCitiesByCountry("#ReceiverCityId", shipment.receiver?.countryId, shipment.receiver?.cityId)
    ]);

    $("#SenderName").val(shipment.sender?.senderName || "");
    $("#SenderContact").val(shipment.sender?.contact || "");
    $("#SenderEmail").val(shipment.sender?.email || "");
    $("#SenderPhone").val(shipment.sender?.phone || "");
    $("#SenderAddress").val(shipment.sender?.address || "");
    $("#SenderAddressDetails").val(shipment.sender?.addressDetails || "");
    $("#SenderOtherAddress").val(shipment.sender?.otherAddress || "");
    $("#SenderPostalCode").val(shipment.sender?.postalCode || "");
    $("#SenderIsDefault").prop("checked", Boolean(shipment.sender?.isDefault));

    $("#ReceiverName").val(shipment.receiver?.receiverName || "");
    $("#ReceiverEmail").val(shipment.receiver?.email || "");
    $("#ReceiverPhone").val(shipment.receiver?.phone || "");
    $("#ReceiverAddress").val(shipment.receiver?.address || "");
    $("#ReceiverAddressDetails").val(shipment.receiver?.addressDetails || "");
    $("#ReceiverOtherAddress").val(shipment.receiver?.otherAddress || "");
    $("#ReceiverPostalCode").val(shipment.receiver?.postalCode || "");
    $("#ReceiverIsDefault").prop("checked", Boolean(shipment.receiver?.isDefault));

    $("#ShippingPackagingId").val(shipment.shippingPackagingId || "");
    $("#Weight").val(shipment.weight ?? "");
    $("#Length").val(shipment.length ?? "");
    $("#Width").val(shipment.width ?? "");
    $("#Height").val(shipment.height ?? "");
    $("#PackageValue").val(shipment.packageValue ?? "");
    $("#ShippingDate").val(formatDateTimeLocal(shipment.shippingDate));
    $("#DeliveryDate").val(formatDateTimeLocal(shipment.deliveryDate));
    $("#ShippingTypeId").val(shipment.shippingTypeId || "");
    $("#PaymentMethodId").val(shipment.paymentMethodId || "");
}
function getShipmentIdFromQuery() {
    const params = new URLSearchParams(window.location.search);
    return params.get("ShId");
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
    const current = editShipmentState.currentShipment || {};

    return {
        id: current.id || editShipmentState.shipmentId,
        shippingDate: $("#ShippingDate").val(),
        deliveryDate: $("#DeliveryDate").val(),
        shippingTypeId: $("#ShippingTypeId").val(),
        shippingPackagingId: getGuidOrNull("#ShippingPackagingId"),
        paymentMethodId: getGuidOrNull("#PaymentMethodId"),
        userSubscriptionId: current.userSubscriptionId || null,
        referenceId: current.referenceId || null,
        width: getNumberValue("#Width"),
        height: getNumberValue("#Height"),
        weight: getNumberValue("#Weight"),
        length: getNumberValue("#Length"),
        packageValue: getNumberValue("#PackageValue"),
        trackingNumber: current.trackingNumber ?? null,
        senderId: current.senderId || current.sender?.id || null,
        receiverId: current.receiverId || current.receiver?.id || null,
        sender: {
            id: current.sender?.id || null,
            userId: current.sender?.userId || null,
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
            id: current.receiver?.id || null,
            userId: current.receiver?.userId || null,
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

function submitShipmentUpdate() {
    const payload = buildShipmentPayload();

    if (!validateShipmentPayload(payload)) {
        AppHelper.showToast("Please complete all required shipment fields before updating.", "error");
        return;
    }

    disableUpdateButton(true, "Updating...");

    ShipmentsService.edit(payload, function (res) {
        const message = res.message || res.Message || "Shipment updated successfully";
        AppHelper.showSuccessAndRedirect(message, `/Shipments/Show?ShId=${payload.id}`);
    }, function (xhr) {
        const message = xhr.responseJSON?.message || xhr.responseJSON?.Message || "Error updating shipment";
        AppHelper.showToast(message, "error");
        console.log(xhr);
        disableUpdateButton(false, "Update Shipment");
    });
}

function formatDateTimeLocal(value) {
    if (!value) {
        return "";
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
        return "";
    }

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");

    return `${year}-${month}-${day}T${hours}:${minutes}`;
}

function setStatus(message, isSuccess = false, isError = false) {
    const element = $("#EditShipmentStatus");
    element.text(message);
    element.removeClass("text-muted text-success text-danger");

    if (isError) {
        element.addClass("text-danger");
        return;
    }

    if (isSuccess) {
        element.addClass("text-success");
        return;
    }

    element.addClass("text-muted");
}

function disableUpdateButton(isDisabled, text) {
    const button = $("#UpdateShipmentBtn");
    button.prop("disabled", isDisabled).val(text);
}
