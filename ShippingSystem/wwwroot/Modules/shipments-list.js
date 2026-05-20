const shipmentsState = {
    pageNumber: 1,
    pageSize: 10,
    totalPages: 0,
    totalCount: 0
};

$(document).ready(function () {
    loadUserShipments(1);

    $(document).on("click", "#shipmentsPagination [data-page]", function (event) {
        event.preventDefault();

        const page = Number($(this).data("page"));
        const pageItem = $(this).closest(".page-item");

        if (pageItem.hasClass("disabled") || !page || page < 1 || page > Math.max(shipmentsState.totalPages, 1) || page === shipmentsState.pageNumber) {
            return;
        }

        loadUserShipments(page);
    });
});

$(document).on("click", ".delete-btn", function (event) {
    event.preventDefault();

    if ($(this).hasClass("disabled")) {
        return;
    }

    const shId = $(this).data("id");

    if (shId && confirm("Are you sure you want to delete this shipment?")) {
        deleteShipment(shId);
    }
});

function deleteShipment(shId) {
    ShipmentsService.delete(shId,
        function () {
            alert("Shipment deleted successfully");
            loadUserShipments(shipmentsState.pageNumber);
        },
        function () {
            alert("Failed to delete shipment");
        }
    );
}

function getPagedResult(res) {
    return {
        items: res?.items || res?.Items || [],
        pageNumber: Number(res?.pageNumber || res?.PageNumber || 1),
        pageSize: Number(res?.pageSize || res?.PageSize || 10),
        totalCount: Number(res?.totalCount || res?.TotalCount || 0),
        totalPages: Number(res?.totalPages || res?.TotalPages || 0),
        hasPreviousPage: Boolean(res?.hasPreviousPage ?? res?.HasPreviousPage ?? false),
        hasNextPage: Boolean(res?.hasNextPage ?? res?.HasNextPage ?? false)
    };
}

function loadUserShipments(pageNumber) {
    $("#shipmentsTableBody").html(`
        <tr>
            <td colspan="15" class="shipment-loading-cell">
                <span class="shipment-loader-dot"></span>
                Loading shipments...
            </td>
        </tr>
    `);

    ShipmentsService.getAll(pageNumber, function (res) {
        const pagedResult = getPagedResult(res);

        shipmentsState.pageNumber = pagedResult.pageNumber;
        shipmentsState.pageSize = pagedResult.pageSize;
        shipmentsState.totalPages = pagedResult.totalPages;
        shipmentsState.totalCount = pagedResult.totalCount;

        renderShipments(pagedResult.items, pagedResult.pageNumber, pagedResult.pageSize);
        renderPagination(pagedResult);
    }, function () {
        $("#shipmentsTableBody").html(`
            <tr>
                <td colspan="15" class="shipment-empty-cell text-danger">Error loading shipments.</td>
            </tr>
        `);
        renderPagination({ pageNumber: 1, totalPages: 1 });
    });
}

function renderShipments(items, pageNumber, pageSize) {
    const tbody = $("#shipmentsTableBody");
    tbody.empty();

    if (!items || !items.length) {
        tbody.html(`
            <tr>
                <td colspan="15" class="shipment-empty-cell">No shipments found.</td>
            </tr>
        `);
        return;
    }

    const rowStart = ((pageNumber - 1) * pageSize) + 1;

    items.forEach((item, index) => {
        const currentState = Number(item.currentState ?? 0);
        const statusText = getStatusName(currentState, item.currentState);
        const canAct = currentState <= 3;
        const editHref = canAct ? `/Shipments/Edit?ShId=${item.id}` : "javascript:void(0);";
        const editClass = canAct ? "mr-2" : "mr-2 text-muted disabled";
        const editTitle = canAct ? "Edit" : "Edit disabled";
        const deleteClass = canAct ? "text-danger delete-btn" : "text-muted delete-btn disabled";
        const deleteTitle = canAct ? "Delete" : "Delete disabled";

        tbody.append(`
            <tr>
                <td>${rowStart + index}</td>
                <td>
                    <span class="shipment-track-pill">
                        ${item.trackingNumber ? Number(item.trackingNumber).toLocaleString() : "N/A"}
                    </span>
                </td>
                <td>${formatDate(item.shippingDate)}</td>
                <td>${formatDate(item.deliveryDate)}</td>
                <td class="text-left">
                    <strong>${escapeHtml(item.sender?.senderName ?? "N/A")}</strong><br />
                    <small>${escapeHtml(item.sender?.email ?? "")}</small><br />
                    <small>${escapeHtml(item.sender?.phone ?? "")}</small>
                </td>
                <td class="text-left">
                    <strong>${escapeHtml(item.receiver?.receiverName ?? "N/A")}</strong><br />
                    <small>${escapeHtml(item.receiver?.email ?? "")}</small><br />
                    <small>${escapeHtml(item.receiver?.phone ?? "")}</small>
                </td>
                <td>${formatIdentifier(item.shippingTypeId)}</td>
                <td>${formatIdentifier(item.shippingPackagingId)}</td>
                <td>${formatIdentifier(item.paymentMethodId)}</td>
                <td><span class="shipment-status-pill shipment-status-${currentState}">${statusText}</span></td>
                <td>${Number(item.weight ?? 0).toFixed(2)} kg</td>
                <td>${Number(item.width ?? 0).toFixed(2)} x ${Number(item.height ?? 0).toFixed(2)} x ${Number(item.length ?? 0).toFixed(2)}</td>
                <td>${Number(item.packageValue ?? 0).toFixed(2)} SAR</td>
                <td class="text-success font-weight-bold">${Number(item.shippingRate ?? 0).toFixed(2)} SAR</td>
                <td>
                    <a href="/Shipments/Show?ShId=${item.id}" title="View" class="shipment-action-link">
                        <i class="fa fa-eye"></i>
                    </a>
                    <a href="${editHref}" title="${editTitle}" class="shipment-action-link ${editClass}" aria-disabled="${!canAct}">
                        <i class="fa fa-edit"></i>
                    </a>
                    <a href="javascript:void(0);" data-id="${item.id}" title="${deleteTitle}" class="shipment-action-link ${deleteClass}" aria-disabled="${!canAct}">
                        <i class="fa fa-trash"></i>
                    </a>
                </td>
            </tr>
        `);
    });
}

function formatIdentifier(value) {
    if (!value) {
        return "N/A";
    }

    const text = String(value);
    const shortText = text.length > 12 ? `${text.slice(0, 8)}...` : text;

    return `<span class="shipment-guid-text" title="${escapeHtml(text)}">${escapeHtml(shortText)}</span>`;
}

function getStatusName(status, fallback) {
    const names = {
        0: "Created",
        1: "Deleted",
        2: "Approved",
        3: "Ready",
        4: "Shipped",
        5: "Delivered",
        6: "Returned",
        7: "Cancelled"
    };

    return escapeHtml(names[status] || fallback || "N/A");
}

function renderPagination(pagedResult) {
    const container = $("#shipmentsPagination");
    container.empty();

    const totalPages = Math.max(1, Number(pagedResult.totalPages || 0));
    const currentPage = Math.min(Math.max(1, Number(pagedResult.pageNumber || 1)), totalPages);
    const hasPreviousPage = currentPage > 1;
    const hasNextPage = currentPage < totalPages;

    const prevDisabled = hasPreviousPage ? "" : " disabled";
    const nextDisabled = hasNextPage ? "" : " disabled";

    let pagesHtml = "";

    for (let page = 1; page <= totalPages; page++) {
        const activeClass = page === currentPage ? " active" : "";
        pagesHtml += `
            <li class="page-item${activeClass}">
                <a class="page-link" href="#" data-page="${page}">${page}</a>
            </li>
        `;
    }

    container.html(`
        <nav aria-label="Shipment pagination" class="d-flex flex-column align-items-center gap-2">
            <ul class="pagination mb-0">
                <li class="page-item${prevDisabled}">
                    <a class="page-link" href="#" data-page="${currentPage - 1}">Previous</a>
                </li>
                ${pagesHtml}
                <li class="page-item${nextDisabled}">
                    <a class="page-link" href="#" data-page="${currentPage + 1}">Next</a>
                </li>
            </ul>
            <small class="text-muted">Page ${currentPage} of ${totalPages}</small>
        </nav>
    `);
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

function escapeHtml(value) {
    return String(value)
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/\"/g, "&quot;")
        .replace(/'/g, "&#39;");
}
