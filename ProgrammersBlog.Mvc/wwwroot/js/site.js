﻿function convertFirstLetterToUpperCase(text) {
    return text.charAt(0).toUpperCase() + text.slice(1);
}
function convertToShortDate(dataString) {
    const shortDate = new Date(dateString).toLocaleDateString('en-US');
    return shortDate;
}