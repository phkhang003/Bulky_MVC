$(document).ready(function () {
    $(".add-to-cart-button").click(function (e) {
        e.preventDefault();
        var productId = $(this).data("product-id");

        $.ajax({
            url: "/Cart/AddToCart",
            type: "POST",
            data: { id: productId },
            success: function (response) {
                if (response.success) {
                    alert("Product added to cart!");
                    // C?p nh?t UI gi? hàng ho?c th?c hi?n hành ??ng khác n?u c?n
                } else {
                    alert("Failed to add product to cart.");
                }
            },
            error: function () {
                alert("Error adding product to cart.");
            }
        });
    });
});
