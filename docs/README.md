# How to contribute

The Fluent Assertions [landing page](https://fluentassertions.com) is using Jekyll and Github Pages.

## How to Build this Site

### Prerequisites

* Ruby 3.1. 
    * An easy way to install is to use `choco install ruby`.
    * Or use the **Ruby+Devkit installer** from [RubyInstaller for Windows](https://rubyinstaller.org/downloads/archives/).
    * Note that you may have to reopen your command shell to get the `ruby --version` command to work,
* The `bundler` 
    * Run `gem install bundler` to install it. If you receive SSL-related errors while running gem install, try running `refreshenv` first.

### Building

* Clone this repository
* Run `bundle install`
* Run `bundle exec jekyll serve` or `run.bat`. 

## Troubleshooting

* Do you receive an error around `jekyll-remote-theme` and `libcurl`? See [this issue on the pages-gem repo](https://github.com/github/pages-gem/issues/526).
* Do you receive an error `Liquid Exception: SSL_connect returned=1 errno=0 state=error: certificate verify failed`? Check out [this solution in the Jekyll repo.](https://github.com/jekyll/jekyll/issues/3985#issuecomment-294266874)
