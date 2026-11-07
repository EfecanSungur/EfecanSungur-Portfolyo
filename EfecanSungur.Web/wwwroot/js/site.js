/* ==========================================================================
   efecansungur.com — arayüz davranışları (bağımlılıksız, defer ile yüklenir)
   Yalnızca: html.js kapısı · üst bar durumu · mobil çekmece · bir kez beliren
   içerik · sayaçlar · referans filtre zenginleştirmesi (+ kapalıysa açılış perdesi).
   Her modül kendi elemanı yoksa sessizce çıkar. JS hiç çalışmasa da site eksiksiz.
   ========================================================================== */
(function () {
    'use strict';

    var root = document.documentElement;
    root.classList.add('js');

    var reduced = window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    var hasIO = 'IntersectionObserver' in window;
    var each = function (list, fn) { Array.prototype.forEach.call(list, fn); };

    /* ------------------------------------------- 0. açılış perdesi (ayar açıksa) */
    (function intro() {
        var el = document.getElementById('intro');
        if (!el) return;
        var seen = false;
        try { seen = sessionStorage.getItem('es-intro') === '1'; } catch (e) { }
        if (seen || reduced) { el.classList.add('is-done'); return; }
        window.setTimeout(function () {
            el.classList.add('is-done');
            try { sessionStorage.setItem('es-intro', '1'); } catch (e) { }
        }, 700);
    })();

    /* ------------------------------------------------------- 1. üst bar durumu */
    /* Ana sayfada koyu hero üstündeki şeffaf bar, kaydırınca düz zemine geçer. */
    (function header() {
        var bars = document.querySelectorAll('[data-hdr].hdr--over');
        if (!bars.length) return;
        var ticking = false;
        function update() {
            var solid = window.scrollY > 8 || document.body.classList.contains('drawer-open');
            each(bars, function (b) { b.classList.toggle('is-solid', solid); });
            ticking = false;
        }
        window.addEventListener('scroll', function () {
            if (!ticking) { ticking = true; window.requestAnimationFrame(update); }
        }, { passive: true });
        update();
    })();

    /* --------------------------------------------------------- 2. mobil çekmece */
    (function drawer() {
        var burger = document.getElementById('burger');
        var panel = document.getElementById('drawer');
        var close = document.getElementById('drawerClose');
        if (!burger || !panel) return;
        var timer = null;

        function open() {
            window.clearTimeout(timer);
            panel.hidden = false;
            // bir kare sonra görünür yap (opaklık geçişi çalışsın)
            window.requestAnimationFrame(function () { panel.classList.add('is-open'); });
            document.body.classList.add('drawer-open');
            burger.setAttribute('aria-expanded', 'true');
            burger.setAttribute('aria-label', 'Menüyü kapat');
            var first = panel.querySelector('.drawer__nav a') || close;
            if (first) first.focus();
        }
        function shut(returnFocus) {
            panel.classList.remove('is-open');
            document.body.classList.remove('drawer-open');
            burger.setAttribute('aria-expanded', 'false');
            burger.setAttribute('aria-label', 'Menüyü aç');
            timer = window.setTimeout(function () { panel.hidden = true; }, reduced ? 0 : 230);
            if (returnFocus) burger.focus();
        }
        function isOpen() { return burger.getAttribute('aria-expanded') === 'true'; }

        burger.addEventListener('click', function () { isOpen() ? shut(true) : open(); });
        if (close) close.addEventListener('click', function () { shut(true); });

        panel.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') { shut(true); return; }
            if (e.key !== 'Tab') return;
            // odak çekmecenin içinde dolaşsın
            var f = panel.querySelectorAll('a[href], button:not([disabled])');
            if (!f.length) return;
            var firstEl = f[0], lastEl = f[f.length - 1];
            if (e.shiftKey && document.activeElement === firstEl) { e.preventDefault(); lastEl.focus(); }
            else if (!e.shiftKey && document.activeElement === lastEl) { e.preventDefault(); firstEl.focus(); }
        });

        window.addEventListener('resize', function () {
            if (isOpen() && window.matchMedia('(min-width: 861px)').matches) shut(false);
        });
    })();

    /* ------------------------------------------------- 3. bir kez beliren içerik */
    /* Ekranda olan öğeler gizlenmeden önce görünür işaretlenir → titreme yok.
       Gizleme ancak bundan sonra (html.reveal) başlar. IntersectionObserver 4 sn
       içinde hiç geri çağırmazsa (ör. arka plan sekmesi) her şey açılır. */
    (function reveal() {
        var items = document.querySelectorAll('[data-reveal]');
        if (!items.length || reduced || !hasIO) return;

        var vh = window.innerHeight || document.documentElement.clientHeight;
        var pending = [];
        each(items, function (el) {
            var r = el.getBoundingClientRect();
            if (r.top < vh && r.bottom > 0) el.classList.add('is-in');
            else pending.push(el);
        });
        if (!pending.length) return;

        root.classList.add('reveal');
        var fired = false;

        var io = new IntersectionObserver(function (entries) {
            fired = true;
            entries.forEach(function (entry) {
                if (!entry.isIntersecting) return;
                var el = entry.target;
                var sibs = el.parentElement ? el.parentElement.querySelectorAll(':scope > [data-reveal]') : [];
                var i = Math.max(0, Array.prototype.indexOf.call(sibs, el));
                el.style.setProperty('--d', Math.min(i, 4) * 70 + 'ms');
                el.classList.add('is-in');
                io.unobserve(el);
            });
        }, { rootMargin: '0px 0px -6% 0px', threshold: 0.06 });

        pending.forEach(function (el) { io.observe(el); });

        window.setTimeout(function () {
            if (fired) return;
            pending.forEach(function (el) { el.classList.add('is-in'); });
            io.disconnect();
        }, 4000);

        window.addEventListener('beforeprint', function () {
            each(items, function (el) { el.classList.add('is-in'); });
        });
    })();

    /* ------------------------------------------------------------ 4. sayaçlar */
    /* Sunucu GERÇEK değeri basar. Sayaç ancak burada başlarsa 0'a çekilir; yıl gibi
       değerlere (1900–2100) sunucu data-count hiç koymaz. 4 sn içinde başlamayan
       sayaç animasyonsuz gerçek değere oturur. */
    (function counters() {
        var nodes = document.querySelectorAll('[data-count]');
        if (!nodes.length) return;

        function finish(el) {
            el.dataset.counted = '1';
            var t = parseFloat(el.getAttribute('data-count'));
            if (!isNaN(t)) el.textContent = String(t);
        }
        function run(el) {
            if (el.dataset.counted) return;
            var target = parseFloat(el.getAttribute('data-count'));
            if (isNaN(target) || reduced || target <= 1) { finish(el); return; }
            el.dataset.counted = '1';
            var dur = 1100, start = null;
            function step(ts) {
                if (start === null) start = ts;
                var p = Math.min((ts - start) / dur, 1);
                var eased = 1 - Math.pow(1 - p, 3);
                el.textContent = String(Math.round(target * eased));
                if (p < 1) window.requestAnimationFrame(step);
                else el.textContent = String(target);
            }
            el.textContent = '0';
            window.requestAnimationFrame(step);
        }

        if (!hasIO) { each(nodes, finish); return; }

        var io = new IntersectionObserver(function (entries) {
            entries.forEach(function (e) {
                if (e.isIntersecting) { run(e.target); io.unobserve(e.target); }
            });
        }, { threshold: 0.5 });
        each(nodes, function (el) { io.observe(el); });

        window.setTimeout(function () {
            each(nodes, function (el) { if (!el.dataset.counted) finish(el); });
        }, 4000);
    })();

    /* --------------------------------------------- 5. referans filtre (zenginleştirme) */
    /* Bağlantılar (?kategori=web) JS olmadan da çalışır; sunucu eşleşmeyen kartları
       hidden ile basar. JS varken sayfa yenilenmeden filtrelenir ve adres güncellenir. */
    (function referenceFilter() {
        var bar = document.querySelector('[data-filter]');
        var grid = document.querySelector('[data-filter-grid]');
        if (!bar || !grid) return;
        var cards = grid.querySelectorAll('[data-cat]');
        var empty = document.querySelector('[data-filter-empty]');
        var count = document.querySelector('[data-filter-count]');

        function apply(key) {
            var shown = 0;
            each(cards, function (c) {
                var match = !key || c.getAttribute('data-cat') === key;
                c.hidden = !match;
                if (match) {
                    shown++;
                    // Gizliyken gözlemlenemeyen kart görünür olsun (reveal beklemesin).
                    c.classList.add('is-in');
                }
            });
            each(bar.querySelectorAll('a[data-key]'), function (x) {
                if ((x.getAttribute('data-key') || '') === key) x.setAttribute('aria-current', 'true');
                else x.removeAttribute('aria-current');
            });
            if (empty) empty.hidden = shown > 0;
            if (count) count.textContent = String(shown);
        }

        // Adres ?kategori=... ile senkron: paylaşılan/yenilenen bağlantı aynı filtreyi açar.
        function setUrl(key) {
            try {
                var url = new URL(window.location.href);
                if (key) url.searchParams.set('kategori', key); else url.searchParams.delete('kategori');
                window.history.replaceState({ kategori: key }, '', url.pathname + url.search + url.hash);
            } catch (err) {
                // Çok eski tarayıcı / kısıtlı çerçeve: bağlantının kendi href'i ile dene.
                try {
                    window.history.replaceState({ kategori: key }, '', key ? '/referanslar?kategori=' + encodeURIComponent(key) : '/referanslar');
                } catch (err2) { }
            }
        }

        bar.addEventListener('click', function (e) {
            var a = e.target.closest ? e.target.closest('a[data-key]') : null;
            if (!a || e.metaKey || e.ctrlKey || e.shiftKey || e.altKey || e.button !== 0) return;
            e.preventDefault();
            var key = a.getAttribute('data-key') || '';
            apply(key);
            setUrl(key);
        });

        // Geri/ileri: adresteki kategoriye göre yeniden uygula.
        window.addEventListener('popstate', function () {
            var key = '';
            try { key = new URL(window.location.href).searchParams.get('kategori') || ''; } catch (err) { }
            apply(key);
        });
    })();

})();
