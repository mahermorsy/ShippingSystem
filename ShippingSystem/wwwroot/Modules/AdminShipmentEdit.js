const adminEditState = {
    shipmentId: null,
    currentShipment: null
};

$(document).ready(function () {
    if (typeof SHIPMENT_ID === 'undefined' || !SHIPMENT_ID) {
        AppHelper.showToast('Shipment ID is missing.', 'error');
        return;
    }
    adminEditState.shipmentId = SHIPMENT_ID;
    adminTagWorkflowFields();
    bindAdminEditEvents();
    initializeAdminEdit();
});

function bindAdminEditEvents() {
    $('#SenderCountryId').on('change', function () {
        adminLoadCities('#SenderCityId', $(this).val());
    });
    $('#ReceiverCountryId').on('change', function () {
        adminLoadCities('#ReceiverCityId', $(this).val());
    });
    $('#AdminSaveShipmentBtn').on('click', adminSubmitSave);
    $('#ApplyStatusBtn').on('click', adminApplyStatus);
}

async function initializeAdminEdit() {
    setAdminStatus('Loading shipment data...');
    adminSetSaveDisabled(true);

    try {
        const [countries, shippingTypes, packaging, paymentMethods, carriers, shipment] = await Promise.all([
            adminGetCountries(),
            adminGetShippingTypes(),
            adminGetPackaging(),
            adminGetPaymentMethods(),
            adminGetCarriers(),
            adminGetShipment(adminEditState.shipmentId)
        ]);

        adminFillDropdown('#SenderCountryId', countries, 'countryEname', 'Select country');
        adminFillDropdown('#ReceiverCountryId', countries, 'countryEname', 'Select country');
        adminFillDropdown('#ShippingTypeId', shippingTypes, 'shippingTypeEname', 'Select type');
        adminFillDropdown('#ShippingPackagingId', packaging, 'shippingPackagingEname', 'Select packaging');
        adminFillDropdown('#PaymentMethodId', paymentMethods, 'methodEname', 'Select method');
        adminFillDropdown('#CarrierId', carriers, 'carrierName', 'Select carrier');

        await adminPopulateForm(shipment);

        setAdminStatus('Shipment loaded.', true);
    } catch (err) {
        console.error(err);
        setAdminStatus('Error loading shipment data.', false, true);
        AppHelper.showToast('Failed to load shipment data.', 'error');
    }
}

/* ---------- Helpers ---------- */

function adminFillDropdown(selector, items, textField, placeholder, valueField = 'id') {
    const ddl = $(selector);
    ddl.empty().append(`<option value="">${placeholder}</option>`);
    (items || []).forEach(item => {
        ddl.append(`<option value="${item[valueField]}">${item[textField] ?? ''}</option>`);
    });
}

function adminGetData(res) {
    return res?.data || res?.Data || [];
}

function adminGetSingle(res) {
    return res?.data || res?.Data || res;
}

function adminLoadCities(selector, countryId, selectedCityId = null) {
    if (!countryId) {
        $(selector).empty().append('<option value="">Select city</option>');
        return Promise.resolve();
    }
    return new Promise((resolve, reject) => {
        CitiesService.getByCountryId(countryId, function (res) {
            adminFillDropdown(selector, adminGetData(res), 'cityNameEn', 'Select city', 'cityId');
            if (selectedCityId) $(selector).val(selectedCityId);
            resolve();
        }, reject);
    });
}

function adminGetCountries() {
    return new Promise((resolve, reject) => {
        CountriesService.getAll(res => resolve(adminGetData(res)), reject);
    });
}

function adminGetShippingTypes() {
    return new Promise((resolve, reject) => {
        ShippingTypesService.getAll(res => resolve(adminGetData(res)), reject);
    });
}

function adminGetPackaging() {
    return new Promise((resolve, reject) => {
        ShippingPackagingService.getAll(res => resolve(adminGetData(res)), reject);
    });
}

function adminGetPaymentMethods() {
    return new Promise((resolve, reject) => {
        PaymentMethodsService.getAll(res => resolve(adminGetData(res)), reject);
    });
}

function adminGetCarriers() {
    return new Promise((resolve, reject) => {
        CarriersService.getAll(res => resolve(adminGetData(res)), reject);
    });
}

function adminGetShipment(id) {
    return new Promise((resolve, reject) => {
        ApiClient.get(`/api/Shipments/admin/${id}`, function (res) {
            const data = adminGetSingle(res);
            if (!data) {
                reject(new Error('Shipment not found'));
                return;
            }
            resolve(data);
        }, reject, true);
    });
}

/* ---------- Populate form ---------- */

async function adminPopulateForm(shipment) {
    adminEditState.currentShipment = shipment;
    const currentState = shipment.currentState ?? shipment.CurrentState ?? 0;

    const tracking = shipment.trackingNumber || 'N/A';
    $('#TrackingBadge').text('Tracking: ' + tracking).removeClass('bg-secondary').addClass('bg-info');

    if (typeof ShipmentStatusTracker !== 'undefined') {
        ShipmentStatusTracker.render(currentState);
    }

    $('#SenderCountryId').val(shipment.sender?.countryId || '');
    $('#ReceiverCountryId').val(shipment.receiver?.countryId || '');

    await Promise.all([
        adminLoadCities('#SenderCityId', shipment.sender?.countryId, shipment.sender?.cityId),
        adminLoadCities('#ReceiverCityId', shipment.receiver?.countryId, shipment.receiver?.cityId)
    ]);

    $('#SenderName').val(shipment.sender?.senderName || '');
    $('#SenderContact').val(shipment.sender?.contact || '');
    $('#SenderEmail').val(shipment.sender?.email || '');
    $('#SenderPhone').val(shipment.sender?.phone || '');
    $('#SenderAddress').val(shipment.sender?.address || '');
    $('#SenderAddressDetails').val(shipment.sender?.addressDetails || '');
    $('#SenderOtherAddress').val(shipment.sender?.otherAddress || '');
    $('#SenderPostalCode').val(shipment.sender?.postalCode || '');
    $('#SenderIsDefault').prop('checked', Boolean(shipment.sender?.isDefault));

    $('#ReceiverName').val(shipment.receiver?.receiverName || '');
    $('#ReceiverEmail').val(shipment.receiver?.email || '');
    $('#ReceiverPhone').val(shipment.receiver?.phone || '');
    $('#ReceiverAddress').val(shipment.receiver?.address || '');
    $('#ReceiverAddressDetails').val(shipment.receiver?.addressDetails || '');
    $('#ReceiverOtherAddress').val(shipment.receiver?.otherAddress || '');
    $('#ReceiverPostalCode').val(shipment.receiver?.postalCode || '');
    $('#ReceiverIsDefault').prop('checked', Boolean(shipment.receiver?.isDefault));

    $('#Weight').val(shipment.weight ?? '');
    $('#Length').val(shipment.length ?? '');
    $('#Width').val(shipment.width ?? '');
    $('#Height').val(shipment.height ?? '');
    $('#PackageValue').val(shipment.packageValue ?? '');
    $('#ShippingRate').val(shipment.shippingRate ?? '');
    $('#ShippingPackagingId').val(shipment.shippingPackagingId || '');
    $('#ShippingTypeId').val(shipment.shippingTypeId || '');
    $('#PaymentMethodId').val(shipment.paymentMethodId || '');
    $('#CarrierId').val(shipment.carrierId || '');
    $('#ShippingDate').val(adminFormatDateTimeLocal(shipment.shippingDate));
    $('#DeliveryDate').val(adminFormatDateTimeLocal(shipment.deliveryDate));

    // في آخر adminPopulateForm بعد كل الـ val() calls
    adminBuildStatusDropdown(currentState);
    adminApplyFieldPermissions(currentState);
}

/* ---------- Build payload ---------- */

function adminBuildPayload() {
    const current = adminEditState.currentShipment || {};
    const sender = current.sender || {};
    const receiver = current.receiver || {};

    return {
        id: current.id || adminEditState.shipmentId,
        shippingDate: $('#ShippingDate').val(),
        deliveryDate: $('#DeliveryDate').val(),
        shippingTypeId: adminGuidValue($('#ShippingTypeId').val(), current.shippingTypeId),
        shippingPackagingId: adminGuidValue($('#ShippingPackagingId').val(), current.shippingPackagingId),
        paymentMethodId: adminGuidValue($('#PaymentMethodId').val(), current.paymentMethodId),
        carrierId: adminGuidValue($('#CarrierId').val(), current.carrierId),
        shippingRate: adminNumberValue($('#ShippingRate').val(), current.shippingRate, null),
        userSubscriptionId: adminGuidValue(current.userSubscriptionId),
        referenceId: adminGuidValue(current.referenceId),
        width: adminNumberValue($('#Width').val(), current.width, 0),
        height: adminNumberValue($('#Height').val(), current.height, 0),
        weight: adminNumberValue($('#Weight').val(), current.weight, 0),
        length: adminNumberValue($('#Length').val(), current.length, 0),
        packageValue: adminNumberValue($('#PackageValue').val(), current.packageValue, 0),
        trackingNumber: current.trackingNumber ?? null,
        senderId: adminGuidValue(current.senderId, sender.id),
        receiverId: adminGuidValue(current.receiverId, receiver.id),
        sender: {
            ...sender,
            id: adminGuidValue(sender.id, current.senderId),
            userId: adminGuidValue(sender.userId),
            senderName: $('#SenderName').val(),
            email: $('#SenderEmail').val(),
            phone: $('#SenderPhone').val(),
            contact: $('#SenderContact').val(),
            cityId: adminGuidValue($('#SenderCityId').val(), sender.cityId),
            countryId: adminGuidValue($('#SenderCountryId').val(), sender.countryId),
            address: $('#SenderAddress').val(),
            addressDetails: $('#SenderAddressDetails').val(),
            otherAddress: $('#SenderOtherAddress').val(),
            isDefault: $('#SenderIsDefault').is(':checked'),
            postalCode: $('#SenderPostalCode').val()
        },
        receiver: {
            ...receiver,
            id: adminGuidValue(receiver.id, current.receiverId),
            userId: adminGuidValue(receiver.userId),
            receiverName: $('#ReceiverName').val(),
            email: $('#ReceiverEmail').val(),
            phone: $('#ReceiverPhone').val(),
            cityId: adminGuidValue($('#ReceiverCityId').val(), receiver.cityId),
            countryId: adminGuidValue($('#ReceiverCountryId').val(), receiver.countryId),
            address: $('#ReceiverAddress').val(),
            addressDetails: $('#ReceiverAddressDetails').val(),
            otherAddress: $('#ReceiverOtherAddress').val(),
            isDefault: $('#ReceiverIsDefault').is(':checked'),
            postalCode: $('#ReceiverPostalCode').val()
        }
    };
}

function adminBuildStagePayload(overrides = {}) {
    const current = adminEditState.currentShipment || {};
    const payload = {
        id: current.id || adminEditState.shipmentId,
        shippingDate: adminDateValue(current.shippingDate, $('#ShippingDate').val(), new Date().toISOString()),
        deliveryDate: adminDateValue(current.deliveryDate, $('#DeliveryDate').val(), new Date().toISOString()),
        shippingTypeId: adminRequiredGuidValue(current.shippingTypeId, $('#ShippingTypeId').val()),
        shippingPackagingId: adminGuidValue(current.shippingPackagingId, $('#ShippingPackagingId').val()),
        paymentMethodId: adminGuidValue(current.paymentMethodId, $('#PaymentMethodId').val()),
        carrierId: adminGuidValue(current.carrierId, $('#CarrierId').val()),
        shippingRate: adminNumberValue(current.shippingRate, $('#ShippingRate').val(), 0),
        userSubscriptionId: adminGuidValue(current.userSubscriptionId),
        referenceId: adminGuidValue(current.referenceId),
        width: adminPositiveNumberValue(current.width, $('#Width').val()),
        height: adminPositiveNumberValue(current.height, $('#Height').val()),
        weight: adminPositiveNumberValue(current.weight, $('#Weight').val()),
        length: adminPositiveNumberValue(current.length, $('#Length').val()),
        packageValue: adminPositiveNumberValue(current.packageValue, $('#PackageValue').val()),
        trackingNumber: current.trackingNumber ?? null,
        senderId: adminRequiredGuidValue(current.senderId, current.sender?.id),
        receiverId: adminRequiredGuidValue(current.receiverId, current.receiver?.id)
    };

    if (Object.prototype.hasOwnProperty.call(overrides, 'carrierId')) {
        payload.carrierId = adminGuidValue(overrides.carrierId, current.carrierId);
    }

    if (Object.prototype.hasOwnProperty.call(overrides, 'deliveryDate')) {
        payload.deliveryDate = adminDateValue(overrides.deliveryDate, current.deliveryDate);
    }

    return payload;
}

/* ---------- Save ---------- */

function adminSubmitSave() {
    const currentState = adminEditState.currentShipment?.currentState ?? adminEditState.currentShipment?.CurrentState ?? 0;
    if (!adminCanEditDetails(currentState)) {
        AppHelper.showToast('Only a reviewer can edit shipment details before approval.', 'warning');
        return;
    }

    const payload = adminBuildPayload();
    const validationMessage = adminValidateShipmentDetails(payload);
    if (validationMessage) {
        AppHelper.showToast(validationMessage, 'warning');
        return;
    }

    adminSetSaveDisabled(true, true);

    ShipmentsService.edit(payload, function (res) {
        const msg = res?.message || res?.Message || 'Shipment updated successfully.';
        AppHelper.showToast(msg, 'success');
        adminSetSaveDisabled(false);
    }, function (xhr) {
        const msg = xhr?.responseJSON?.message || xhr?.responseJSON?.Message || 'Failed to update shipment.';
        AppHelper.showToast(msg, 'error');
        adminSetSaveDisabled(false);
    });
}

/* ---------- Status change ---------- */

function adminApplyStatus() {
    const newStatus = $('#NewStatusSelect').val();
    if (!newStatus) {
        AppHelper.showToast('Please select a status to apply.', 'warning');
        return;
    }

    const statusInt = parseInt(newStatus);
    const current = adminEditState.currentShipment;
    const currentState = current?.currentState ?? current?.CurrentState ?? 0;

    if (statusInt === 2) {
        const payload = adminBuildPayload();
        payload.currentState = 2;

        const validationMessage = adminValidateShipmentDetails(payload);
        if (validationMessage) {
            AppHelper.showToast(validationMessage, 'warning');
            return;
        }

        adminPostWorkflowStatus(payload, 2, 'Shipment approved.', 'Failed to approve shipment.');
    }
    else if (statusInt === 3) {
        const payload = adminBuildStatusPayload(3, { carrierId: $('#CarrierId').val() });
        if (!payload.carrierId) {
            AppHelper.showToast('Please select a carrier first.', 'warning');
            return;
        }

        adminPostWorkflowStatus(payload, 3, 'Shipment is ready for shipment.', 'Failed to mark shipment as ready.');
    }
    else if (statusInt === 4) {
        const payload = adminBuildStatusPayload(4, { deliveryDate: $('#DeliveryDate').val() });
        if (!payload.deliveryDate) {
            AppHelper.showToast('Please select a delivery date first.', 'warning');
            return;
        }
        if (new Date(payload.deliveryDate) <= new Date()) {
            AppHelper.showToast('Delivery date must be in the future.', 'warning');
            return;
        }

        adminPostWorkflowStatus(payload, 4, 'Shipment marked as shipped.', 'Failed to mark shipment as shipped.');
    }
    else if (statusInt === 7) {
        if (currentState >= 4) {
            AppHelper.showToast('Shipped shipments cannot be cancelled from here.', 'warning');
            return;
        }

        adminPostWorkflowStatus(adminBuildStatusPayload(7), 7, 'Shipment cancelled.', 'Failed to cancel shipment.');
    }
}

function adminBuildStatusPayload(nextState, overrides = {}) {
    const payload = adminBuildStagePayload(overrides);
    payload.currentState = nextState;
    return payload;
}

function adminPostWorkflowStatus(payload, nextState, successMessage, errorMessage) {
    $('#ApplyStatusBtn').prop('disabled', true);
    console.log('Shipment workflow payload:', payload);

    ApiClient.post('/api/Shipments/ChangeStatus', payload, function (res) {
        const msg = res?.message || res?.Message || successMessage;
        AppHelper.showToast(msg, 'success');

        adminEditState.currentShipment = {
            ...(adminEditState.currentShipment || {}),
            ...payload,
            currentState: nextState
        };

        if (typeof ShipmentStatusTracker !== 'undefined') {
            ShipmentStatusTracker.render(nextState);
        }

        adminBuildStatusDropdown(nextState);
        adminApplyFieldPermissions(nextState);
        $('#ApplyStatusBtn').prop('disabled', false);
    }, function (xhr) {
        const serverErrors = adminExtractServerErrors(xhr);
        console.error('Shipment workflow failed:', {
            status: xhr?.status,
            responseJSON: xhr?.responseJSON,
            responseText: xhr?.responseText,
            serverErrors
        });

        const msg =
            serverErrors[0] ||
            xhr?.responseJSON?.message ||
            xhr?.responseJSON?.Message ||
            xhr?.responseJSON?.title ||
            errorMessage;

        AppHelper.showToast(msg, 'error');
        $('#ApplyStatusBtn').prop('disabled', false);
    });
}

/* ---------- Utilities ---------- */

function adminTagWorkflowFields() {
    $('#CarrierId').closest('.col-md-3').attr('data-workflow-field', 'carrier');
    $('#DeliveryDate').closest('.col-md-3').attr('data-workflow-field', 'delivery-date');
}

function adminGuidValue(...values) {
    const emptyGuid = '00000000-0000-0000-0000-000000000000';
    const guidPattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;

    for (const value of values) {
        if (typeof value !== 'string') continue;

        const trimmed = value.trim();
        if (trimmed && trimmed !== emptyGuid && guidPattern.test(trimmed)) {
            return trimmed;
        }
    }

    return null;
}

function adminRequiredGuidValue(...values) {
    return adminGuidValue(...values) || '00000000-0000-0000-0000-000000000000';
}

function adminNumberValue(value, fallbackValue, defaultValue) {
    const parsed = parseFloat(value);
    if (!Number.isNaN(parsed)) return parsed;

    const fallback = parseFloat(fallbackValue);
    if (!Number.isNaN(fallback)) return fallback;

    return defaultValue;
}

function adminPositiveNumberValue(value, fallbackValue) {
    const parsed = adminNumberValue(value, fallbackValue, 0.01);
    return parsed > 0 ? parsed : 0.01;
}

function adminDateValue(...values) {
    for (const value of values) {
        if (!value) continue;

        const date = new Date(value);
        if (!isNaN(date.getTime())) {
            return value;
        }
    }

    return '';
}

function adminIsValidEmail(value) {
    if (!value) return false;
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value.trim());
}

function adminValidateShipmentDetails(payload) {
    if (!payload.id) return 'Shipment ID is missing.';
    if (!payload.shippingDate) return 'Please select a shipping date.';
    if (!payload.shippingTypeId) return 'Please select a shipping type.';
    if (payload.weight <= 0) return 'Weight must be greater than 0.';
    if (payload.length <= 0) return 'Length must be greater than 0.';
    if (payload.width <= 0) return 'Width must be greater than 0.';
    if (payload.height <= 0) return 'Height must be greater than 0.';
    if (payload.packageValue <= 0) return 'Declared value must be greater than 0.';
    if (!payload.senderId || !payload.receiverId) return 'Sender and receiver IDs are missing.';
    if (!payload.sender?.senderName || payload.sender.senderName.trim().length < 3) return 'Sender name must be at least 3 characters.';
    if (!payload.receiver?.receiverName || payload.receiver.receiverName.trim().length < 3) return 'Receiver name must be at least 3 characters.';
    if (!payload.sender?.email) return 'Sender email is required.';
    if (!adminIsValidEmail(payload.sender.email)) return 'Please enter a valid sender email address.';
    if (!payload.receiver?.email) return 'Receiver email is required.';
    if (!adminIsValidEmail(payload.receiver.email)) return 'Please enter a valid receiver email address.';
    if (!payload.sender?.phone || payload.sender.phone.trim().length < 7) return 'Sender phone must be at least 7 characters.';
    if (!payload.receiver?.phone || payload.receiver.phone.trim().length < 7) return 'Receiver phone must be at least 7 characters.';
    if (!payload.sender?.countryId || !payload.sender?.cityId) return 'Sender country and city are required.';
    if (!payload.receiver?.countryId || !payload.receiver?.cityId) return 'Receiver country and city are required.';
    if (!payload.sender?.address || payload.sender.address.trim().length < 5) return 'Sender address must be at least 5 characters.';
    if (!payload.receiver?.address || payload.receiver.address.trim().length < 5) return 'Receiver address must be at least 5 characters.';

    return null;
}

function adminExtractServerErrors(xhr) {
    const response = xhr?.responseJSON || {};
    const errors = response.errors || response.Errors;

    if (!errors) return [];
    if (Array.isArray(errors)) return errors;

    return Object.values(errors)
        .flat()
        .filter(Boolean);
}

function adminFormatDateTimeLocal(value) {
    if (!value) return '';
    const d = new Date(value);
    if (isNaN(d.getTime())) return '';
    const p = n => String(n).padStart(2, '0');
    return `${d.getFullYear()}-${p(d.getMonth() + 1)}-${p(d.getDate())}T${p(d.getHours())}:${p(d.getMinutes())}`;
}

function setAdminStatus(msg, isSuccess = false, isError = false) {
    const el = $('#AdminEditStatus');
    el.text(msg).removeClass('text-muted text-success text-danger');
    if (isError) el.addClass('text-danger');
    else if (isSuccess) el.addClass('text-success');
    else el.addClass('text-muted');
}

function adminSetSaveDisabled(disabled, loading = false) {
    $('#AdminSaveShipmentBtn').prop('disabled', disabled);
    if (loading) $('#AdminSaveSpinner').removeClass('d-none');
    else $('#AdminSaveSpinner').addClass('d-none');
}

/* ---------- Role → Status options ---------- */
const ROLE_STATUS_MAP = {
    'Reviewer': [
        { value: 2, label: 'Approved' },
        { value: 7, label: 'Cancelled' }
    ],
    'Operation': [
        { value: 3, label: 'Ready for Shipment' },
        { value: 7, label: 'Cancelled' }
    ],
    'Operation Manager': [
        { value: 4, label: 'Shipped' },
        { value: 7, label: 'Cancelled' }
    ],
    'Admin': [
        { value: 2, label: 'Approved' },
        { value: 3, label: 'Ready for Shipment' },
        { value: 4, label: 'Shipped' },
        { value: 7, label: 'Cancelled' }
    ]
};

function adminBuildStatusDropdown(currentState) {
    const select = $('#NewStatusSelect');
    select.empty().append('<option value="">-- Select new status --</option>');

    const options = ROLE_STATUS_MAP[USER_ROLE] || [];

    if (options.length === 0) {
        $('#StatusChangeCard').hide();
        return;
    }

    options.forEach(opt => {
        // بتخبي الـ option لو الشحنة وصلت له بالفعل أو أعلى منه
        if (opt.value <= currentState && opt.value !== 7) return;
        select.append(`<option value="${opt.value}">${opt.label}</option>`);
    });
}

function adminCanEditDetails(currentState) {
    return (USER_ROLE === 'Admin' || USER_ROLE === 'Reviewer') && currentState === 0;
}

function adminCanEditCarrier(currentState) {
    return (USER_ROLE === 'Admin' || USER_ROLE === 'Operation') && currentState === 2;
}

function adminCanEditDeliveryDate(currentState) {
    return (USER_ROLE === 'Admin' || USER_ROLE === 'Operation Manager') && currentState === 3;
}

function adminApplyFieldPermissions(currentState) {
    $('.edit-field').prop('disabled', true);
    $('#NewStatusSelect').prop('disabled', false);
    $('[data-workflow-field="carrier"], [data-workflow-field="delivery-date"]').hide();

    if (adminCanEditDetails(currentState)) {
        $('.edit-field').prop('disabled', false);
        $('#CarrierId, #DeliveryDate').prop('disabled', true);
    }

    if (adminCanEditCarrier(currentState)) {
        $('[data-workflow-field="carrier"]').show();
        $('#CarrierId').prop('disabled', false);
    }

    if (adminCanEditDeliveryDate(currentState)) {
        $('[data-workflow-field="delivery-date"]').show();
        $('#DeliveryDate').prop('disabled', false);
    }

    adminSetSaveDisabled(!adminCanEditDetails(currentState));
}

const ROLE_TRANSITIONS = {
    'Reviewer': [
        { from: 0, value: 2, label: 'Approved' },
        { from: 0, value: 7, label: 'Cancelled' }
    ],
    'Operation': [
        { from: 2, value: 3, label: 'Ready for Shipment' },
        { from: 2, value: 7, label: 'Cancelled' }
    ],
    'Operation Manager': [
        { from: 3, value: 4, label: 'Shipped' },
        { from: 3, value: 7, label: 'Cancelled' }
    ],
    'Admin': [
        { from: 0, value: 2, label: 'Approved' },
        { from: 2, value: 3, label: 'Ready for Shipment' },
        { from: 3, value: 4, label: 'Shipped' },
        { from: 0, value: 7, label: 'Cancelled' },
        { from: 2, value: 7, label: 'Cancelled' },
        { from: 3, value: 7, label: 'Cancelled' }
    ]
};

function adminBuildStatusDropdown(currentState) {
    const select = $('#NewStatusSelect');
    select.empty().append('<option value="">-- Select new status --</option>');

    const options = (ROLE_TRANSITIONS[USER_ROLE] || []).filter(opt => opt.from === currentState);

    if (options.length === 0) {
        $('#StatusChangeCard').hide();
        return;
    }

    $('#StatusChangeCard').show();
    options.forEach(opt => {
        select.append(`<option value="${opt.value}">${opt.label}</option>`);
    });
}
