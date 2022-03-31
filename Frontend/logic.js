$(window).on('load', function () {
    $.get("http://localhost:5207/User", function (data, status) {
        console.log(data);
        data.forEach(record => {
            insertNewRecord(record);
        });
    });
});


var selectedRow = null

function onFormSubmit() {
    if (validate()) {
        var formData = readFormData();
        if (selectedRow == null)
            insertNewRecordWithFormData(formData);
        else
            updateRecord(formData);
    }
}

function readFormData() {
    var formData = new FormData();
    formData.append("cn", document.getElementById("cn").value);
    formData.append("sn", document.getElementById("sn").value);
    formData.append("password", document.getElementById("userPassword").value);
    return formData;
}

function insertNewRecord(data) {
    console.log(data);
    var table = document.getElementById("ldapList").getElementsByTagName('tbody')[0];
    var newRow = table.insertRow(table.length);
    cell1 = newRow.insertCell(0);
    cell1.innerHTML = data.cn;
    cell2 = newRow.insertCell(1);
    cell2.innerHTML = data.sn;
    cell2 = newRow.insertCell(2);
    cell2.innerHTML = data.password;
    cell4 = newRow.insertCell(3);
    cell4.innerHTML = `<div><button type="button" onClick="onEdit(this)" class="btn btn-warning" style="margin-right: 5px")>Editieren</button><button type="button" onClick="onDelete(this)" class="btn btn-danger">Löschen</button></div>`;
}

function insertNewRecordWithFormData(formData) {
    var data = {};
    data.cn = formData.get("cn");
    data.sn = formData.get("sn");
    data.password = formData.get("password");
    $.ajax({
        url: `http://localhost:5207/User`,
        type: 'POST',
        data: data,
        success: function (result) {
            var table = document.getElementById("ldapList").getElementsByTagName('tbody')[0];
            var newRow = table.insertRow(table.length);
            cell1 = newRow.insertCell(0);
            cell1.innerHTML = formData.get("cn");
            cell2 = newRow.insertCell(1);
            cell2.innerHTML = formData.get("sn");
            cell2 = newRow.insertCell(2);
            cell2.innerHTML = formData.get("password");
            cell4 = newRow.insertCell(3);
            cell4.innerHTML = `<div><button type="button" onClick="onEdit(this)" class="btn btn-warning" style="margin-right: 5px")>Editieren</button><button type="button" onClick="onDelete(this)" class="btn btn-danger">Löschen</button></div>`;
        },
        error: function (xhr, status, error) {
            alert("Fehler!!!: " + xhr.responseText);
        }
    });
}

function resetForm() {
    document.getElementById("submitButton").innerHTML = "Hinzufügen";
    document.getElementById("cn").value = "";
    document.getElementById("sn").value = "";
    document.getElementById("userPassword").value = "";
    document.getElementById("cn").removeAttribute("readonly");
    selectedRow = null;
}

function onEdit(td) {
    selectedRow = td.parentElement.parentElement.parentElement;
    $("#submitButton").html("Bearbeiten");
    document.getElementById("cn").setAttribute("readonly", true);
    document.getElementById("cn").value = selectedRow.cells[0].innerHTML;
    document.getElementById("sn").value = selectedRow.cells[1].innerHTML;
    document.getElementById("userPassword").value = selectedRow.cells[2].innerHTML;

}
function updateRecord(formData) {
    var cn = selectedRow.cells[0].innerHTML;
    $.ajax({
        url: `http://localhost:5207/User/${cn}`,
        type: 'PUT',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            selectedRow.cells[0].innerHTML = formData.get("cn");
            selectedRow.cells[1].innerHTML = formData.get("sn");
            console.log(selectedRow.cells[2].innerHTM);
            selectedRow.cells[2].innerHTML = formData.get("password");
            resetForm();
        },
        error: function (xhr, status, error) {
            alert("Fehler!!!: " + xhr.responseText);
        }
    });

}



function onDelete(td) {
    var row = td.parentElement.parentElement.parentElement;

    if (confirm('Wollen sie sicher ' + row.cells[0].innerHTML + ' löschen?')) {
        row = td.parentElement.parentElement.parentElement;
        $.ajax({
            url: `http://localhost:5207/User/${row.cells[0].innerHTML}`,
            type: 'DELETE',
            success: function (result) {
                document.getElementById("ldapList").deleteRow(row.rowIndex);
                resetForm();
            },
            error: function (xhr, status, error) {
                alert("Fehler!!!: " + xhr.responseText);
            }
        });

    }
}
function validate() {
    isValid = true;
    if (document.getElementById("cn").value == "") {
        isValid = false;
    }
    return isValid;
}