$(document).ready(function () {
    fetchEvents();
    $('#eventForm').submit(function (e) {
        e.preventDefault();

        const eventData = {
            Name: $('#eventTitle').val(),
            StartDate:$('#startTime').val(),  // Convert to ISO format
            EndDate: $('#endTime').val(),   
            EventType: $('#eventType').val(), 
            Description: $('#eventDescription').val(),

        };

        $.ajax({
            url: '/Admin/CreateEvent', // URL to your POST API endpoint
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(eventData),
            success: function (response) {
                $('#eventModal').modal('hide'); // Hide modal
                alert('Event added successfully!');
                fetchCalendarData(); // Reload calendar to show new event
            },
            error: function () {
                alert('Failed to add event.');
            }
        });
    });
    let currentMonth = new Date().getMonth();
    let currentYear = new Date().getFullYear();

    function fetchCalendarData() {
        $.ajax({
            url: '/Admin/Calendar2', // Controller endpoint that returns the JSON data
            method: 'GET',
            success: function (classes) {
                renderCalendar(currentMonth, currentYear, classes);
            },
            error: function () {
                alert('Failed to fetch class data.');
            }
        });
    }

    function renderCalendar(month, year, events) {
        const monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        $('#monthYear').text(`${monthNames[month]} ${year}`);

        const firstDay = new Date(year, month, 1).getDay();
        const daysInMonth = new Date(year, month + 1, 0).getDate();

        let calendarDays = '';
        let date = 1;

        for (let i = 0; i < 6; i++) {
            let row = '<tr>';
            for (let j = 0; j < 7; j++) {
                if (i === 0 && j < firstDay) {
                    row += '<td></td>';
                } else if (date > daysInMonth) {
                    break;
                } else {
                    row += `<td data-date="${year}-${month + 1}-${date}" class="calendar-cell">${date}`;

                    const eventsOnThisDay = events.filter(e => {
                        const eventDate = new Date(e.startDate);
                        return eventDate.getDate() === date && eventDate.getMonth() === month && eventDate.getFullYear() === year;
                    });

                    eventsOnThisDay.forEach(event => {
                        row += `<div class="event-item">${event.name}</div>`;
                    });

                    row += '</td>';
                    date++;
                }
            }
            row += '</tr>';
            calendarDays += row;
        }

        $('#calendarDays').html(calendarDays);

        // Add click event to open modal on date click
        $('.calendar-cell').click(function () {
            const selectedDate = $(this).data('date');
            $('#eventDate').val(selectedDate); // Set selected date in the hidden input
            $('#eventModal').modal('show');    // Show modal
        });
    }



    // Navigate to previous month
    $('#prevMonth').click(function () {
        currentMonth--;
        if (currentMonth < 0) {
            currentMonth = 11;
            currentYear--;
        }
        fetchCalendarData();
    });

    // Navigate to next month
    $('#nextMonth').click(function () {
        currentMonth++;
        if (currentMonth > 11) {
            currentMonth = 0;
            currentYear++;
        }
        fetchCalendarData();
    });

    // Initial load
    fetchCalendarData();
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


function fetchEvents() {
    $.ajax({
        url: '/Admin/GetAllEvents2', // Adjust the API endpoint as needed
        method: 'GET',
        success: function (data) {
            let tableBody = $('#eventsTable tbody');
            tableBody.empty();  // Clear any existing rows

            data.forEach(event => {
                let row = `
                    <tr>
                        <td>${event.name}</td>
                        <td>${event.eventType}</td>
                        <td>${new Date(event.startDate).toLocaleString()}</td>
                        <td>${new Date(event.endDate).toLocaleString()}</td>
                        <td>${event.description}</td>
                        <td>
                            <button class="btn btn-primary" onclick="editEvent(${event.eventId})">Edit</button>
                            <button class="btn btn-danger" onclick="deleteEvent(${event.eventId})">Delete</button>
                        </td>
                    </tr>
                `;
                tableBody.append(row);  // Add the row to the table
            });
        },
        error: function () {
            alert('Failed to load events.');
        }
    });
}



function editEvent(eventId) {
    // Fetch event details by ID and populate the modal fields
    $.ajax({
        url: `/Admin/GetEvent?id=${eventId}`,
        method: 'GET',
        success: function (event) {
            $('#eventTitle').val(event.name);
            $('#eventType').val(event.eventType);
            $('#startTime').val(new Date(event.startDate).toISOString().slice(0, 16)); // Format for datetime-local input
            $('#endTime').val(new Date(event.endDate).toISOString().slice(0, 16));
            $('#eventDescription').val(event.description);

            $('#eventModal').modal('show');  // Open modal for editing
        },
        error: function () {
            alert('Failed to fetch event details.');
        }
    });

    // Submit form to save edited event
    $('#eventForm').off('submit').on('submit', function (e) {
        e.preventDefault();

        const updatedEventData = {
            EventId: eventId,  // Ensure the ID is sent for updating
            Name: $('#eventTitle').val(),
            EventType: $('#eventType').val(),
            StartDate: new Date($('#startTime').val()).toISOString(),
            EndDate: new Date($('#endTime').val()).toISOString(),
            Description: $('#eventDescription').val(),
        };

        $.ajax({
            url: `/Admin/EditEvent?id=${eventId}`, // Adjust the API endpoint as needed
            method: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(updatedEventData),
            success: function () {
                $('#eventModal').modal('hide');  // Hide modal after saving
                fetchEvents();  // Reload the events table
                alert('Event updated successfully!');
            },
            error: function () {
                alert('Failed to update event.');
            }
        });
    });
}



function deleteEvent(eventId) {
    if (confirm('Are you sure you want to delete this event?')) {
        $.ajax({
            url: `/Admin/DeleteEvent?id=${eventId}`,
            method: 'DELETE',
            success: function () {
                alert('Event deleted successfully!');
                fetchEvents();  // Reload the events table
            },
            error: function () {
                alert('Failed to delete event.');
            }
        });
    }
}
