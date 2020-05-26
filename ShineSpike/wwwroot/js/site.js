(function () {
    // Delete post
    var deleteButton = edit.querySelector("#delete-post");
    if (deleteButton) {
        deleteButton.addEventListener("click", function (e) {
            if (!confirm("Are you sure you want to delete the post?")) {
                e.preventDefault();
            }
        });
    }
})();
