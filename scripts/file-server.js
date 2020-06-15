'use strict';

const http = require('http');
const fs = require('fs');
const path = require('path');
const Promisify = require('util').promisify;

const root = path.join(process.env.HOME, 'Music', 'iTunes', 'iTunes Media', 'Music');

const { URLSearchParams } = require('url');

const port = process.env.PORT || 3000;

process.on('uncaughtException', console.error);
process.on('unhandledRejection', console.error);

const server = http.createServer(async function(req, res) {
  if (!req.method === 'GET'){
    res.statusCode = 405;
    return res.end(`Only /GET :${port}/${artist}/${album}/${song} is allowed`);
  }

  console.info(`[+] Received query for ${req.url}`);

  try {
    console.info('Query', optsFromQuery(req.url));
    const filePath = findMusicPath(optsFromQuery(req.url));
    confirmPath(filePath);
    try {
      const rawData = await Promisify(fs.readFile)(filePath);
      const [_, ...extension] = path.extname(filePath);
      // res.setHeader('Content-Type', extension.join(''));
      return res.end(rawData);
    } catch (err) {
      res.statusCode = 500;
      return res.end(JSON.stringify({ message: 'Failed to send song', error: err.message }));
    }
  } catch (err) {
    res.statusCode = 404;
    return res.end(JSON.stringify({ message: 'Song not found', error: err.message }));
  }
});

server.on('clientError', err => console.error(err.message));

function findMusicPath({ artist, album, song }) {
  // returns path to music
  return path.join(root, artist, album, song);
}

function optsFromQuery(candidate) {
  const url = candidate.split('/')[1];
  const searchParams = new URLSearchParams(url);

  const [ artist, album, song ] = [searchParams.get('artist'), searchParams.get('album'), searchParams.get('song')];
  return {
    artist, album, song
  }
}

function confirmPath(fullPath) {
  Promisify(fs.access)(fullPath, fs.constants.R_OK || fs.constants.F_OK);
}

server.listen(port, () => {
  console.info(`Dev Server is up and running on http://localhost:${port}`);
});
