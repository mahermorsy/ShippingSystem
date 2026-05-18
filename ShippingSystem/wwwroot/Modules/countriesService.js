const CountriesService = {
    getAll: function (onSuccess, onError) {
        ApiClient.get('/api/Countries', onSuccess, onError, false);
    },

    getById: function (id, onSuccess, onError) {
        ApiClient.get(`/api/Countries/${id}`, onSuccess, onError, false);
    }
};