const CitiesService = {
    getAll: function (onSuccess, onError) {
        ApiClient.get('/api/Cities', onSuccess, onError, false);
    },

    getById: function (id, onSuccess, onError) {
        ApiClient.get(`/api/Cities/${id}`, onSuccess, onError, false);
    },

    getByCountryId: function (id, onSuccess, onError) {
        ApiClient.get(`/api/Cities/Country/${id}`, onSuccess, onError, false);
    }
};
