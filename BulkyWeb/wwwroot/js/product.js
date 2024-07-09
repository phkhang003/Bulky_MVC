var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { data: 'title', "width": "25%" },
            { data: 'isbn', "width": "15%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'author', "width": "15%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>Edit</a>
                    <button class="btn btn-danger mx-2 btn-delete" data-url="/admin/product/delete/${data}">
                        <i class="bi bi-trash-fill"></i> Delete
                    </button>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });

    // Bắt sự kiện click cho nút xóa
    $('#tblData').on('click', '.btn-delete', function () {
        var url = $(this).data('url');
        Delete(url);
    });
}

function Delete(url) {
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
