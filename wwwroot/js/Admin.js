$(document).ready(function () {
    $.ajax({
        url: "/Admin/GetTeacher",
        type: "Get",
        dataType: "json",
        success: function (result) {
            var htmlContent = "";
            var i = 1;
            $.each(result, function (index, item) {
                console.log("Processing item:", item);

                htmlContent += "<option value='" + item.teacherId + "'>" + item.firstName + "</option>";
                i++;
            });
            console.log(htmlContent);
            $('#TeacherId').html(htmlContent);
        },
        error: function (xhr, status, error) {


            console.error("AJAX Error:", status, error);
            console.error("Response Text:", xhr.responseText);
        }
    })

    $.ajax({
        url: "/Admin/GetClass",
        type: "Get",
        dataType: "json",
        success: function (result) {
            var htmlContent = "";
            var i = 1;
            $.each(result, function (index, item) {
                console.log("Processing item:", item);
               
                htmlContent += "<option value='" + item.className + "'>" + item.className + "</option>";
                i++;
            });
            console.log(htmlContent);
            $('#AddClassInFees').html(htmlContent);
            $('#GetclassinStudent').html(htmlContent);
        },
        error: function (xhr, status, error) {  


            console.error("AJAX Error:", status, error);
            console.error("Response Text:", xhr.responseText);
        }
    })


    $.ajax({
        url: "/Admin/GetNoOfStudents",
        type: "Get",
        dataType: "json",
        success: function (result) {

            console.log(result);
            $('#noofstudents').html(result);
        },
        error: function () { 
            console.log("DOne")
        }
    })

    $.ajax({
        url: "/Admin/NoOfTeachers",
        type: "Get",
        dataType: "json",
        success: function (result) {

            console.log(result);
            $('#NoOfTeachers').html(result);
        },
        error: function () { 
            console.log("DOne")
        }
    })
});

