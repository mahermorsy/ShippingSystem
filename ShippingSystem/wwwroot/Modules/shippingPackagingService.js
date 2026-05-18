const ShippingPackagingService = {

    getAll: function (onSuccess, onError) {
        ApiClient.get('/api/ShippingPackaging', onSuccess, onError, false);
    },

    getById: function (id, onSuccess, onError) {
        ApiClient.get(`/api/ShippingPackaging/${id}`, onSuccess, onError, false);
    }

};