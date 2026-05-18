const CarriersService = {
    getAll: function (onSuccess, onError) {
        ApiClient.get('/api/Carriers', onSuccess, onError, false);
    },

    getById: function (id, onSuccess, onError) {
        ApiClient.get(`/api/Carriers/${id}`, onSuccess, onError, false);
    }
};