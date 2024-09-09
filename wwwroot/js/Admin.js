$(document).ready(function () {
    $.ajax({
        url: "https://localhost:44386/api/Admin/GetTeacher",
        type: "Get",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        success: function (result) {
            var htmlContent = "";
            var i = 1;
            $.each(result, function (index, item) {
                console.log("Processing item:", item);
               
                htmlContent += "<option value='" + item.TeacherId + "'>" + item.FirstName + "</option>";
                i++;
            });
            console.log(htmlContent);
            $('#TeacherId').html(htmlContent);
        },
        error: function () { 
            console.log("DOne")
        }
    })


    $.ajax({
        url: "https://localhost:44386/api/Admin/GetNoOfStudents",
        type: "Get",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        success: function (result) {

            console.log(result);
            $('#noofstudents').html(result);
        },
        error: function () { 
            console.log("DOne")
        }
    })

    $.ajax({
        url: "https://localhost:44386/api/Admin/NoOfTeachers",
        type: "Get",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        success: function (result) {

            console.log(result);
            $('#NoOfTeachers').html(result);
        },
        error: function () { 
            console.log("DOne")
        }
    })
});

