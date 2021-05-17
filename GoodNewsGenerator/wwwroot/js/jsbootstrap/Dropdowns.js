

var dropdownElementList = [].slice.call(document.querySelectorAll('.btn .btn-danger .dropdown-toggle'))
var dropdownList = dropdownElementList.map(function (dropdownToggleEl) {
    return new bootstrap.Dropdown(dropdownToggleEl)
})