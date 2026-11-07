/* ==========================================================================
   Yönetim paneli — arayüz davranışları
   ========================================================================== */
(function () {
    'use strict';

    /* --------------------------------------------------------- 1. sidebar */
    (function side() {
        var burger = document.getElementById('admBurger');
        var scrim = document.getElementById('admScrim');
        if (!burger) return;

        function toggle() { document.body.classList.toggle('adm-open'); }
        function shut() { document.body.classList.remove('adm-open'); }

        burger.addEventListener('click', toggle);
        if (scrim) scrim.addEventListener('click', shut);
        document.addEventListener('keydown', function (e) { if (e.key === 'Escape') shut(); });
        window.addEventListener('resize', function () {
            if (window.innerWidth > 900) shut();
        });
    })();

    /* ------------------------------------------------------- 2. flash kapat */
    (function flash() {
        var box = document.querySelector('.adm__flash');
        if (!box) return;
        var x = box.querySelector('.adm__flashx');
        if (x) x.addEventListener('click', function () { box.remove(); });
        window.setTimeout(function () {
            if (document.body.contains(box)) {
                box.style.transition = 'opacity .4s, transform .4s';
                box.style.opacity = '0';
                box.style.transform = 'translateY(-8px)';
                window.setTimeout(function () { box.remove(); }, 420);
            }
        }, 6000);
    })();

    /* ---------------------------------------------- 3. renk seçici eşleştirme */
    (function colors() {
        var wraps = document.querySelectorAll('.colorf');
        Array.prototype.forEach.call(wraps, function (w) {
            var picker = w.querySelector('input[type=color]');
            var text = w.querySelector('input[type=text]');
            if (!picker || !text) return;

            picker.addEventListener('input', function () { text.value = picker.value.toUpperCase(); });
            text.addEventListener('input', function () {
                var v = text.value.trim();
                if (/^#([0-9a-fA-F]{6}|[0-9a-fA-F]{3})$/.test(v)) picker.value = v;
            });
        });
    })();

    /* --------------------------------------------- 4. görsel önizleme (upload) */
    (function images() {
        var inputs = document.querySelectorAll('.imgf input[type=file]');
        Array.prototype.forEach.call(inputs, function (input) {
            input.addEventListener('change', function () {
                var wrap = input.closest('.imgf');
                if (!wrap || !input.files || !input.files[0]) return;

                var file = input.files[0];
                var name = wrap.querySelector('.imgf__name');
                if (name) name.textContent = file.name;

                var box = wrap.querySelector('.imgf__preview');
                if (!box) return;

                var url = URL.createObjectURL(file);
                var img = box.querySelector('img');
                if (!img) {
                    box.innerHTML = '';
                    img = document.createElement('img');
                    box.appendChild(img);
                }
                img.src = url;
                img.alt = file.name;
            });
        });
    })();

    /* ---------------------------------------------------- 5. silme onayı */
    (function confirmDelete() {
        document.addEventListener('submit', function (e) {
            var form = e.target;
            if (!form.matches || !form.matches('form[data-confirm]')) return;
            var msg = form.getAttribute('data-confirm') || 'Bu işlemi onaylıyor musunuz?';
            if (!window.confirm(msg)) e.preventDefault();
        });
    })();

    /* ------------------------------------------------------- 6. mesaj açma */
    (function messages() {
        var heads = document.querySelectorAll('.msg__head');
        Array.prototype.forEach.call(heads, function (h) {
            h.addEventListener('click', function (e) {
                if (e.target.closest('button') || e.target.closest('a')) return;
                var msg = h.closest('.msg');
                if (msg) msg.classList.toggle('is-open');
            });
        });
    })();

    /* -------------------------------------------- 7. kaydetmeden çıkma uyarısı */
    (function dirtyGuard() {
        var form = document.querySelector('form[data-dirty]');
        if (!form) return;

        var dirty = false;
        form.addEventListener('input', function () { dirty = true; });
        form.addEventListener('change', function () { dirty = true; });
        form.addEventListener('submit', function () { dirty = false; });

        window.addEventListener('beforeunload', function (e) {
            if (!dirty) return;
            e.preventDefault();
            e.returnValue = '';
        });
    })();

    /* ------------------------------------------------ 8. şifre göster/gizle */
    (function pwToggle() {
        var btn = document.querySelector('.pweye');
        if (!btn) return;
        var input = document.getElementById('Password');
        if (!input) return;
        btn.addEventListener('click', function () {
            input.type = input.type === 'password' ? 'text' : 'password';
            btn.setAttribute('aria-label', input.type === 'password' ? 'Şifreyi göster' : 'Şifreyi gizle');
        });
    })();

    /* ---------------------------------------- 9. Ctrl+S ile formu kaydet */
    (function quickSave() {
        document.addEventListener('keydown', function (e) {
            if (!(e.ctrlKey || e.metaKey) || e.key.toLowerCase() !== 's') return;
            var form = document.querySelector('form[data-dirty]');
            if (!form) return;
            e.preventDefault();
            form.requestSubmit ? form.requestSubmit() : form.submit();
        });
    })();

})();
