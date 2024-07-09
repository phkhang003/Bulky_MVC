var dataTable;
$(document).ready(function () {
    loadDataTable();
});

var dataTable;

function loadDataTable() {
    dataTable = $('#categoryData').DataTable({
        "ajax": { url: '/admin/category/getall' },
        "columns": [
            { data: 'name', "width": "25%" },
            { data: 'displayOrder', "width": "20%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
            <a href="/Admin/Category/Edit/${data}" class="btn btn-primary mx-2">
                <i class="bi bi-pencil-square"></i> Edit
            </a>
            <button class="btn btn-danger mx-2 btn-delete" data-url="/Admin/Category/Delete/${data}">
                <i class="bi bi-trash-fill"></i> Delete
            </button>
        </div>`;
                },
                "width": "25%"
            }
        ]
    });

    // Bắt sự kiện click cho nút xóa
    $('#categoryData').on('click', '.btn-delete', function () {
        var url = $(this).data('url');
        deleteCategory(url);
    });
}

function deleteCategory(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);

                    // Tự động đóng popup sau khi xóa thành công
                    setTimeout(() => {
                        Swal.close();
                    }, 1500);
                },
                error: function () {
                    toastr.error('Error occurred while processing your request.');
                }
            });
        }
    });
}
