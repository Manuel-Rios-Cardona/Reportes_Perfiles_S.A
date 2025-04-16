$(document).ready(function () {
    // Initialize date inputs with current date
    var today = new Date().toISOString().split('T')[0];
    $('input[type="date"]').each(function () {
        if (!$(this).val()) {
            $(this).val(today);
        }
    });

    // Calculate age when birth date changes
    $('#txtBirthDate').on('change', function () {
        var birthDate = new Date($(this).val());
        var today = new Date();
        var age = today.getFullYear() - birthDate.getFullYear();
        var monthDiff = today.getMonth() - birthDate.getMonth();

        if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }

        // Update age display if it exists
        var ageDisplay = $('#lblAge');
        if (ageDisplay.length) {
            ageDisplay.text(age + ' años');
        }
    });

    // Calculate years of service when hire date changes
    $('#txtHireDate').on('change', function () {
        var hireDate = new Date($(this).val());
        var today = new Date();
        var years = today.getFullYear() - hireDate.getFullYear();
        var monthDiff = today.getMonth() - hireDate.getMonth();

        if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < hireDate.getDate())) {
            years--;
        }

        // Update years of service display if it exists
        var yearsDisplay = $('#lblYearsOfService');
        if (yearsDisplay.length) {
            yearsDisplay.text(years + ' años');
        }
    });

    // Form validation
    $('form').on('submit', function (e) {
        var isValid = true;

        // Required field validation
        $(this).find('[required]').each(function () {
            if (!$(this).val()) {
                isValid = false;
                $(this).addClass('is-invalid');
            } else {
                $(this).removeClass('is-invalid');
            }
        });

        // Date validation
        var birthDate = new Date($('#txtBirthDate').val());
        var hireDate = new Date($('#txtHireDate').val());
        var today = new Date();

        if (birthDate > today) {
            isValid = false;
            $('#txtBirthDate').addClass('is-invalid');
            alert('La fecha de nacimiento no puede ser en el futuro');
        }

        if (hireDate > today) {
            isValid = false;
            $('#txtHireDate').addClass('is-invalid');
            alert('La fecha de ingreso no puede ser en el futuro');
        }

        if (hireDate < birthDate) {
            isValid = false;
            $('#txtHireDate').addClass('is-invalid');
            alert('La fecha de ingreso no puede ser anterior a la fecha de nacimiento');
        }

        // DPI validation (assuming DPI is 13 digits)
        var dpi = $('#txtDPI').val();
        if (dpi && !/^\d{13}$/.test(dpi)) {
            isValid = false;
            $('#txtDPI').addClass('is-invalid');
            alert('El DPI debe contener 13 dígitos');
        }

        if (!isValid) {
            e.preventDefault();
        }
    });

    // Clear form validation on input
    $('input, select').on('input change', function () {
        $(this).removeClass('is-invalid');
    });

    // Confirm delete
    $('.btn-delete').on('click', function (e) {
        if (!confirm('¿Está seguro de eliminar este registro?')) {
            e.preventDefault();
        }
    });

    // Toggle department status confirmation
    $('.btn-toggle').on('click', function (e) {
        var action = $(this).text().toLowerCase();
        if (!confirm('¿Está seguro de ' + action + ' este departamento?')) {
            e.preventDefault();
        }
    });

    // Report date range validation
    $('#btnGenerateReport').on('click', function (e) {
        var startDate = new Date($('#txtStartDate').val());
        var endDate = new Date($('#txtEndDate').val());

        if (startDate > endDate) {
            e.preventDefault();
            alert('La fecha de inicio no puede ser posterior a la fecha de fin');
            $('#txtStartDate').addClass('is-invalid');
            $('#txtEndDate').addClass('is-invalid');
        }
    });
}); 