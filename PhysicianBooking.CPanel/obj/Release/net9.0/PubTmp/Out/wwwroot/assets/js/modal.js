// =========================================================
// confirm-modal.js
// Reusable confirm popup — wires any [data-confirm] element
// to the shared Bootstrap modal instead of native confirm().
//
// Usage on ANY link/button, ANY page:
//
//   <a href="/Faq/Delete/5"
//      data-confirm
//      data-confirm-title="Delete this FAQ?"
//      data-confirm-text="This action can't be undone."
//      data-confirm-btn="Yes, delete"
//      data-confirm-icon="bi-trash3"
//      data-confirm-variant="danger">
//      Delete
//   </a>
//
// For elements that aren't plain navigation links (e.g. a
// delete button inside a <form> that must POST), add
// data-confirm-form="theFormId" instead of relying on href —
// the script will submit that form on confirm.
//
// Include this file once, globally (e.g. in _Layout.cshtml
// via <script src="~/js/confirm-modal.js"></script>), alongside
// the _ConfirmModal partial.
// =========================================================

(function () {
    document.addEventListener('DOMContentLoaded', function () {
        var modalEl = document.getElementById('confirmModal');
        if (!modalEl) return; // partial not included on this page

        var iconEl = document.getElementById('confirmModalIcon');
        var titleEl = document.getElementById('confirmModalTitle');
        var textEl = document.getElementById('confirmModalText');
        var confirmBtn = document.getElementById('confirmModalConfirmBtn');
        var cancelBtn = document.getElementById('confirmModalCancelBtn');

        if (!iconEl || !titleEl || !textEl || !confirmBtn) {
            console.warn(
                '[confirm-modal] #confirmModal was found but is missing expected children ' +
                '(confirmModalIcon / confirmModalTitle / confirmModalText / confirmModalConfirmBtn). ' +
                'Check for a duplicate element with id="confirmModal" elsewhere on the page, ' +
                'or confirm the _ConfirmModal partial rendered correctly.'
            );
            return;
        }

        var modal = new bootstrap.Modal(modalEl);


        document.addEventListener('click', function (e) {
            var trigger = e.target.closest('[data-confirm]');
            if (!trigger) return;

            e.preventDefault();

            var title = trigger.getAttribute('data-confirm-title') || 'Are you sure?';
            var text = trigger.getAttribute('data-confirm-text') || "This action can't be undone.";
            var btnText = trigger.getAttribute('data-confirm-btn') || 'Confirm';
            var icon = trigger.getAttribute('data-confirm-icon') || 'bi-question-circle';
            var variant = trigger.getAttribute('data-confirm-variant') || 'danger'; // danger | primary | warning

            titleEl.textContent = title;
            textEl.textContent = text;
            confirmBtn.textContent = btnText;

            iconEl.className = 'confirm-modal__icon confirm-modal__icon--' + variant;
            iconEl.innerHTML = '<i class="bi ' + icon + '"></i>';

            confirmBtn.className = 'btn confirm-modal__btn confirm-modal__btn--confirm confirm-modal__btn--' + variant;

            // Clear any previous handler
            var newConfirmBtn = confirmBtn.cloneNode(true);
            confirmBtn.parentNode.replaceChild(newConfirmBtn, confirmBtn);
            confirmBtn = newConfirmBtn;

            confirmBtn.addEventListener('click', function () {
                var formId = trigger.getAttribute('data-confirm-form');
                if (formId) {
                    var form = document.getElementById(formId);
                    if (form) form.submit();
                } else if (trigger.tagName === 'A' && trigger.getAttribute('href')) {
                    window.location.href = trigger.getAttribute('href');
                } else if (trigger.tagName === 'BUTTON' && trigger.form) {
                    trigger.form.submit();
                }
                modal.hide();
            });

            cancelBtn.addEventListener('click', function () {
                modal.hide();
            });

            modal.show();
        });
    });
})();