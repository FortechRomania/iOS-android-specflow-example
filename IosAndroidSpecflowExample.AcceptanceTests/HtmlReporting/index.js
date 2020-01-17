const argv = require('yargs').argv
var reporter = require('cucumber-html-reporter');
 
var options = {
        name: argv.name,
        theme: 'bootstrap',
        jsonFile: argv.jsonFile,
        output: argv.outputHtml
    };
 
reporter.generate(options);