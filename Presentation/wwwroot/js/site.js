// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function selectSeat(row, column) {
    // Update hidden fields with the user selected row and column
    document.getElementById('selectedRow').value = row;
    document.getElementById('selectedColumn').value = column;

    // Remove 'selected' class from all seats
    var seats = document.querySelectorAll('.seat-icon');

    seats.forEach(function (seat) {
        seat.classList.remove('selected');
    });

    // Find the clicked seat and add 'selected' class
    var clickedSeat = document.querySelector('.seat-icon[data-row="' + row + '"][data-column="' + column + '"]');
    if (clickedSeat) {
        clickedSeat.classList.add('selected');
    }
}

// Attach the selectSeat function to the window object so it can be accessed globally
window.selectSeat = selectSeat;
