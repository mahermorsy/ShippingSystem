$(document).ready(function () {
    loadShipmentDetails();
});

function loadShipmentDetails() {
    const shipmentId = AppHelper.getQueryParam("ShId") || AppHelper.getIdFromPath();

    if (!shipmentId) {
        renderShipmentError("Shipment id is missing.");
        return;
    }

    ShipmentsService.getById(shipmentId, function (res) {
        renderShipmentDetails(res);
    }, function (xhr) {
        const message = xhr?.responseJSON?.message || xhr?.responseJSON?.Message || "Error loading shipment details.";
        renderShipmentError(message);
    });
}

function renderShipmentDetails(shipment) {
    if (!shipment || !shipment.id) {
        renderShipmentError("Shipment details are not available.");
        return;
    }

    $("#shipmentDetailsAlert")
        .removeClass("shipment-status-error")
        .addClass("shipment-status-success")
        .text("Shipment details loaded successfully.");

    $("#shipmentDetailsContent").removeClass("d-none").show();

    setText("#shipmentTrackingBadge", `Tracking: ${formatTrackingNumber(shipment.trackingNumber)}`);
    setText("#shipmentShippingDate", formatDate(shipment.shippingDate));
    setText("#shipmentDeliveryDate", formatDate(shipment.deliveryDate));
    setText("#shipmentShippingTypeId", shipment.shippingTypeId);
    setText("#shipmentShippingPackagingId", shipment.shippingPackagingId);
    setText("#shipmentPaymentMethodId", shipment.paymentMethodId);
    setText("#shipmentWeight", `${formatNumber(shipment.weight)} kg`);
    setText("#shipmentDimensions", `${formatNumber(shipment.width)} x ${formatNumber(shipment.height)} x ${formatNumber(shipment.length)}`);
    setText("#shipmentPackageValue", `${formatNumber(shipment.packageValue)} SAR`);
    setText("#shipmentShippingRate", `${formatNumber(shipment.shippingRate)} SAR`);

    setText("#shipmentSenderName", shipment.sender?.senderName);
    setText("#shipmentSenderEmail", shipment.sender?.email);
    setText("#shipmentSenderPhone", shipment.sender?.phone);
    setText("#shipmentSenderContact", shipment.sender?.contact);
    setText("#shipmentSenderAddress", shipment.sender?.address);
    setText("#shipmentSenderAddressDetails", shipment.sender?.addressDetails);
    setText("#shipmentSenderOtherAddress", shipment.sender?.otherAddress);
    setText("#shipmentSenderPostalCode", shipment.sender?.postalCode);

    setText("#shipmentReceiverName", shipment.receiver?.receiverName);
    setText("#shipmentReceiverEmail", shipment.receiver?.email);
    setText("#shipmentReceiverPhone", shipment.receiver?.phone);
    setText("#shipmentReceiverAddress", shipment.receiver?.address);
    setText("#shipmentReceiverAddressDetails", shipment.receiver?.addressDetails);
    setText("#shipmentReceiverOtherAddress", shipment.receiver?.otherAddress);
    setText("#shipmentReceiverPostalCode", shipment.receiver?.postalCode);
}

function renderShipmentError(message) {
    $("#shipmentDetailsAlert")
        .removeClass("shipment-status-success")
        .addClass("shipment-status-error")
        .text(message || "Error loading shipment details.");

    $("#shipmentTrackingBadge").text("Tracking: N/A");
    $("#shipmentDetailsContent").addClass("d-none").hide();
}

function setText(selector, value) {
    $(selector).text(value || "N/A");
}

function formatDate(value) {
    if (!value) {
        return "N/A";
    }

    const date = new Date(value);

    if (isNaN(date.getTime())) {
        return "N/A";
    }

    return date.toLocaleDateString("en-GB", {
        day: "2-digit",
        month: "short",
        year: "numeric"
    });
}

function formatNumber(value) {
    const number = Number(value);
    return Number.isFinite(number) ? number.toFixed(2) : "0.00";
}

function formatTrackingNumber(value) {
    if (!value && value !== 0) {
        return "N/A";
    }

    const number = Number(value);
    return Number.isFinite(number) ? number.toLocaleString() : value;
}

