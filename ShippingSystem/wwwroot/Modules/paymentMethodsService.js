const PaymentMethodsService = {

    getAll: function (onSuccess, onError) {
        ApiClient.get('/api/PaymentMethods', onSuccess, onError, false);
    },

    getById: function (id, onSuccess, onError) {
        ApiClient.get(`/api/PaymentMethods/${id}`, onSuccess, onError, false);
    }

};
