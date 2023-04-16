del _site /s /q

rem Documentation spell check
powershell -ExecutionPolicy ByPass -NoProfile -File "..\build.ps1" --target SpellCheck --no-logo

bundle exec jekyll serve --incremental