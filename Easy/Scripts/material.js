$(function () { "object" == typeof ripples && ripples.init(".btn:not(.btn-link), .navbar a:not(.withoutripple), .nav-tabs a:not(.withoutripple), .withripple"); var a = function () { $(".checkbox > label > input").not(".bs-material").addClass("bs-material").after("<span class=check></span>"), $(".radio > label > input").not(".bs-material").addClass("bs-material").after("<span class=circle></span><span class=check></span>"), $("input.form-control, textarea.form-control, select.form-control").not(".bs-material").each(function () { if (!$(this).is(".bs-material")) { if ($(this).wrap("<div class=form-control-wrapper></div>"), $(this).after("<span class=material-input></span>"), $(this).hasClass("floating-label")) { var a = $(this).attr("placeholder"); $(this).attr("placeholder", null).removeClass("floating-label"), $(this).after("<div class=floating-label>" + a + "</div>") } if (($(this).is(":empty") || null === $(this).val() || "undefined" == $(this).val() || "" === $(this).val()) && $(this).addClass("empty"), $(this).parent().next().is("[type=file]")) { $(this).parent().addClass("fileinput"); var b = $(this).parent().next().detach(); $(this).after(b) } } }) }; a(), document.arrive && document.arrive("input, textarea, select", function () { a() }), $(document).on("change", ".checkbox input", function () { $(this).blur() }), $(document).on("keyup change", ".form-control", function () { var a = $(this); setTimeout(function () { "" === a.val() ? a.addClass("empty") : a.removeClass("empty") }, 1) }), $(document).on("focus", ".form-control-wrapper.fileinput", function () { $(this).find("input").addClass("focus") }).on("blur", ".form-control-wrapper.fileinput", function () { $(this).find("input").removeClass("focus") }).on("change", ".form-control-wrapper.fileinput [type=file]", function () { var a = ""; $.each($(this)[0].files, function (b, c) { console.log(c), a += c.name + ", " }), a = a.substring(0, a.length - 2), a ? $(this).prev().removeClass("empty") : $(this).prev().addClass("empty"), $(this).prev().val(a) }) });