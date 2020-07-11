'use strict';

const { By, ThenableDriver, until, WebElement } = require('selenium-webdriver');
const _ = require('lodash');
const HTMLParser = require('fast-html-parser');
const { getDriver } = require('./crawler');
const { randomPause, promiseTimeout } = require('./util');
const { HOME_PAGE, SEARCH_TIMEOUT_MS, ONE_MINUTE_MS, PAGE_LOAD_TIMEOUT_MS } = require('./constants');


function _errorhandler(err) {
  console.error(err);
  console.info('Err name: ', err.name);
  console.info('Err code: ', err.code);
  console.info('Err message: ', err.message);
}

/**
 *
 * @param {ThenableDriver} driver
 */
function visitSite(driver) {
  return driver.get(HOME_PAGE);
}

/**
 * 
 * @param {ThenableDriver} driver 
 * @param {string} searchTerm 
 */
function searchATerm(driver, searchTerm) {
  return driver
    .findElement(By.css('input[type="text"]'))
    .sendKeys(...searchTerm.split(' ').join(' '))
    .then(() => driver.sleep(ONE_MINUTE_MS));
}

/**
 * 
 * @param {ThenableDriver} driver 
 * @param {string} searchTerm 
 */
async function getSearchResults(driver, searchTerm) {
  const results = await driver.findElements(By.css('main ul div div:nth-child(2)'));
  
  // TODO (oneeyedsunday) check text of results for not found errors or search pane not ready errors
  console.log('Getting text of all results: ', await Promise.all(results.map(r => r.getText())));
  return results;
}

/**
 * 
 * @param {ThenableDriver} driver 
 * @param {WebElement} result 
 * @param {string} searchTerm 
 */
async function walkResult(driver, result, searchTerm) {
  await result.click();
  await driver.wait(until.titleContains(searchTerm), SEARCH_TIMEOUT_MS);
}

(async () => {
  const driver = getDriver(true);
  try {
    await driver.manage().setTimeouts({ pageLoad: PAGE_LOAD_TIMEOUT_MS });

    await visitSite(driver);
    await randomPause();

    const searchTerm = 'Solar Plexus';  
    await searchATerm(driver, searchTerm);
    const results = await getSearchResults(driver, searchTerm);
    // TODO (oneeyedsunday) watch for no results

    // This currently assumes the resylts yield an album
    // if search term was an artist, it would show other artists
    await walkResult(driver, results[0], searchTerm);

    

    const songs = await driver.findElements(By.css('main div ul li'));

    const textOfSongs = await Promise.all(songs.map(song => song.getText()));
    console.log('Text of songs under alnum: ', textOfSongs);

    await driver.sleep(ONE_MINUTE_MS);
    await songs[0].click();
    await driver.sleep(ONE_MINUTE_MS);

    /**
     * @type {Array<string>}
     */
    const mediaLinks = await driver.executeScript(function() {
      return performance.getEntriesByType('resource').filter(entry => entry.initiatorType === 'audio')
        .map(data => data.name)
    });

    console.log('Media Links: ', mediaLinks);
  } catch (err) {
    _errorhandler(err);
  } finally {
    return driver.quit();
  }
})()



