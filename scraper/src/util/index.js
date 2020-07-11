'use strict';


function promiseTimeout(time) {
  return new Promise((resolve, reject) => setTimeout(resolve, time))
};

function randomPause() {
  const seconds = ((Math.random() + 1) * 10);

  return promiseTimeout(seconds * 1000);
}

module.exports = {
  promiseTimeout,
  randomPause
};
