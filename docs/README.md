# How to contribute

The Fluent Assertions [landing page](https://fluentassertions.com) is using Jekyll and Github Pages.

## How to Build this Site

### Prerequisites

* Ruby 2.4.x (note: 2.5 and higher may fail due to the `ffi` ruby lib only supporting < 2.5). An easy way to install is to use `choco install ruby --version 2.4.3.1`  
* The `bundler` gem (`gem install bundler`). If you receive SSL-related errors while running `gem install`, try running `refreshenv` first. 

### Building

* Clone this repository
* `cd` into the `root/docs` directory of the repository
* Install the Ruby Devkit using `ridk install` followed by option 3.
* Run `bundle install`. 
* Run `bundle exec jekyll serve`. To have it monitor your working directory for changes, add the `--incremental` option. 

## Troubleshooting

* Do you receive an error around `jekyll-remote-theme` and `libcurl`? See [this issue on the pages-gem repo](https://github.com/github/pages-gem/issues/526).
* Do you receive an error `Liquid Exception: SSL_connect returned=1 errno=0 state=error: certificate verify failed`? Check out [this solution in the Jekyll repo.](https://github.com/jekyll/jekyll/issues/3985#issuecomment-294266874)
* 
