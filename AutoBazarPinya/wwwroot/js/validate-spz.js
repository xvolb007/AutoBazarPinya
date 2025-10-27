$(function () {
    const $spz = $('#LicensePlate');
    const $feedback = $('#spz-feedback');
    const $id = $('#VehicleId');

    $spz.on('blur', function () {
        const spz = $spz.val();
        const id = $id.val();

        if (!spz) return;

        $.get('/Vehicle/CheckLicensePlate', { licensePlate: spz, id: id }, function (isValid) {
            if (!isValid) {
                $spz.addClass('is-invalid');
                $feedback.text('SPZ již existuje v databázi.');
            } else {
                $spz.removeClass('is-invalid');
                $feedback.text('');
            }
        });
    });
});
