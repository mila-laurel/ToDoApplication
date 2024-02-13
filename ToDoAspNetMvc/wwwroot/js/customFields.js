function createField() {
    var elements = document.getElementsByClassName("field_form");
    var count = elements.length;
    var divElement = document.createElement("div");
    divElement.setAttribute("class", "form-group field-form");
    var divId = "Field_Empty_" + count;
    divElement.setAttribute("id", divId);
    document.getElementById("CustomFields").append(divElement)

    var div2Element = document.createElement("div");
    div2Element.setAttribute("style", "display: flex");
    divElement.append(div2Element);

    var newNameLabel = document.createElement("input");
    newNameLabel.setAttribute("class", "control-label");
    newNameLabel.setAttribute("type", "text");
    newNameLabel.setAttribute("id", "Fields_" + count + "__Name");
    newNameLabel.setAttribute("name", "Fields[" + count + "].Name");
    div2Element.append(newNameLabel);

    var button = document.createElement("a");
    button.setAttribute("class", "btn btn-sm DeleteField");
    button.setAttribute("data-toggle", "tooltip");
    button.setAttribute("title", "Delete field");
    button.setAttribute("data-fieldindex", count.toString());
    button.addEventListener(onclick, deleteField(this));
    div2Element.append(button);

    var spanIcon = document.createElement("span");
    button.setAttribute("class", "bi bi-backspace");
    button.setAttribute("style", "color:red");
    button.append(spanIcon);

    var newValueInput = document.createElement("input");
    newValueInput.setAttribute("class", "form-control");
    newValueInput.setAttribute("type", "text");
    newValueInput.setAttribute("id", "Fields_" + count + "__Value");
    newValueInput.setAttribute("name", "Fields[" + count + "].Value");
    divElement.append(newValueInput);
}

function deleteField(e, token) {
    var fieldid = e.getAttribute("data-fieldid");
    if (fieldid) {
        $.ajax({
            url: `/Fields/Delete?id=${fieldid}`,
            headers: { "RequestVerificationToken": token },
            type: "POST",
            cache: false,
            async: true,
            success: function (data) {
                var field = document.getElementById(`Field_${fieldid}`);
                if (field) {
                    field.remove();
                }
            }
        });
    }
    else {
        var fieldid = e.getAttribute("data-fieldindex");
        var field = document.getElementById(`Field_Empty_${fieldid}`);
        if (field) {
            field.remove();
        }
    }
}