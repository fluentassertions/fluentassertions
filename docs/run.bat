del _site /s /q

rem Documentation spell check
..\build.cmd --target SpellCheck

bundle exec jekyll serve --incremental