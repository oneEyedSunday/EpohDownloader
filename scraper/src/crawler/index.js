'use strict';

const { Builder } = require('selenium-webdriver');
const Chrome = require('selenium-webdriver/chrome');
const ChromeDriver = require('chromedriver');
Chrome.setDefaultService(new Chrome.ServiceBuilder(ChromeDriver.path).build());

function getDriver(headless = false) {
  const HUB_HOST = process.env.SELENIUM_HUB_HOST || '127.0.0.1';
  const defaultArgs = [
    '--disable-gpu',
    '--no-sandbox',
    '--disable-dev-shm-usage'
  ];
  const args = headless ? defaultArgs.concat('--headless') : defaultArgs;

  return new Builder()
    .forBrowser('chrome')
    .usingServer(`http://${HUB_HOST}:4444/wd/hub`)
    .setChromeOptions(new Chrome.Options()
      .windowSize({width: 1500, height: 820})
      .addArguments(args))
    .build();
}

module.exports = {
  getDriver
}