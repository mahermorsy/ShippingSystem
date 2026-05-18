const ShippingTypesService = {
    getAll: function (onSuccess, onError) {
        ApiClient.get('/api/ShippingTypes', onSuccess, onError, false);
    },

    getById: function (id, onSuccess, onError) {
        ApiClient.get(`/api/ShippingTypes/${id}`, onSuccess, onError, false);
    }
};