const ShipmentsService = {
    getAll: function (pageNumber, onSuccess, onError) {
        const page = pageNumber && pageNumber > 0 ? pageNumber : 1;
        ApiClient.get(`/api/Shipments?Pagenumber=${page}`, onSuccess, onError, true);
    },

    getById: function (id, onSuccess, onError) {
        ApiClient.get(`/api/Shipments/${id}`, onSuccess, onError, true);
    },

    edit: function (payload, onSuccess, onError) {
        ApiClient.post('/api/Shipments/Edit', payload, onSuccess, onError, true);
    },
    // في ShipmentsService
    delete: function (id, onSuccess, onError) {
        ApiClient.post(`/api/Shipments/Delete?Shid=${encodeURIComponent(id)}`, {}, onSuccess, onError, true);
    }
};
