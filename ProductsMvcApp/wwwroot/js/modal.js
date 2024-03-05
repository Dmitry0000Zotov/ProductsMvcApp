function openModal(params) {
    const id = params.id;
    const url = params.url;
    const isEdit = params.isEdit;
    const modal = $('#mainModal');
    let title;

    if (isEdit) {
        title = "Edit product";
    } else {
        title = "Add product";
    }

    $.ajax({
        type: 'GET',
        url: url,
        data: { "id": id, "isEdit": isEdit },
        success: function (response) {
            modal.find(".modal-title").html(title);
            modal.find(".modal-body").html(response);
            modal.modal('show')
        },
        failure: function () {
            modal.modal('hide')
        },
        error: function (response) {
            alert(response.Description);
        }
    });
};

function openDeleteModal(params) {
    const id = params.id;
    const url = params.url;
    const modal = $('#mainModal');
    let title = "Delete product";

    if (id == null || id === undefined) {
        alert("Something wrong");
    }

    $.ajax({
        type: 'GET',
        url: url,
        data: { "id": id },
        success: function (response) {
            modal.find(".modal-title").html(title);
            modal.find(".modal-body").html(response);
            modal.modal('show')
        },
        failure: function () {
            modal.modal('hide')
        },
        error: function (response) {
            alert(response.Description);
        }
    });
};
