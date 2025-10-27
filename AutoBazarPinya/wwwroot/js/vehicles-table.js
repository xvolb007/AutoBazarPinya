$(document).ready(function () {
    const table = $('#vehiclesTable').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/Vehicle/GetPaged',
            type: 'POST',
            headers: { 'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() }
        },
        columns: [
            { data: 'manufacturer' },
            { data: 'model' },
            { data: 'year' },
            { data: 'licensePlate' },
            { data: 'conditionDisplay' },
            {
                data: 'id',
                orderable: false,
                render: function (data, type, row) {
                    return `
                        <a href="/Vehicle/Details/${data}" class="btn btn-sm btn-outline-primary me-1">Detail</a>
                        <a href="/Vehicle/Edit/${data}" class="btn btn-sm btn-outline-warning me-1">Upravit</a>
                        <button type="button" class="btn btn-sm btn-outline-danger"
                            data-bs-toggle="modal" data-bs-target="#deleteModal"
                            data-id="${data}" data-spz="${row.licensePlate}">
                            Smazat
                        </button>`;
                }
            }
        ],
        language: {
            search: "Hledat:",
            lengthMenu: "Zobrazit _MENU_ vozidel",
            info: "Zobrazuji _START_ až _END_ z _TOTAL_ vozidel",
            infoEmpty: "Žádná vozidla",
            zeroRecords: "Nic nenalezeno",
            paginate: {
                previous: "Předchozí",
                next: "Další"
            }
        }
    });
});
