'use strict';

// Include gulp
var gulp = require('gulp');

// Include Our Plugins
var sasslint = require('gulp-sass-lint'),
    sass = require('gulp-sass'),
    autoprefixer = require('gulp-autoprefixer'),
    cache = require('gulp-cached'),
    sourcemaps = require('gulp-sourcemaps'),
    livereload = require('gulp-livereload');

//Placeholder
var location;

// Lint SCSS
gulp.task('sasslint', function() {
  gulp.src(['dominion.scss', 'new.scss', 'new2.scss'])
    .pipe(cache(sasslint({})))
    .pipe(sasslint.format())
    .pipe(sasslint.failOnError());
});

// Compile our CSS, create source maps
gulp.task('sass', function () {
  return gulp.src('dominion.scss')
    .pipe(sourcemaps.init())
    .pipe(sass({outputStyle: 'compressed'}).on('error', sass.logError))
    .pipe(sourcemaps.write('.'))
    .pipe(gulp.dest('.'))
});

// // Compile our CSS, create source maps for benefits subdirectory
// gulp.task('benefits', function () {
//   return gulp.src('source/segue/benefits/benefits.scss')
//     .pipe(sourcemaps.init())
//     .pipe(sass({outputStyle: 'compressed'}).on('error', sass.logError))
//     .pipe(sourcemaps.write('.'))
//     .pipe(gulp.dest('source/segue/benefits'))
// });

// Compile our CSS, create source maps - dev work only
gulp.task('dev', function () {
  return gulp.src('dominion.scss')
    .pipe(sourcemaps.init())
    .pipe(sass().on('error', sass.logError))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest('.'))
});

// Auto-add vendor prefixes
gulp.task('autoprefix', function() {
    return gulp.src('dominion.css')
    .pipe(sourcemaps.init({loadMaps: true}))
    .pipe(autoprefixer({
      browsers: [
        '> 1%',
        'last 2 versions',
        'firefox >= 4',
        'safari 7',
        'safari 8',
        'IE 8',
        'IE 9',
        'IE 10',
        'IE 11'
      ],
      cascade: false
    }))
    .pipe(sourcemaps.write('.'))
    .pipe(gulp.dest('.'))
    .pipe(livereload());
});

// Watch files for changes
gulp.task('watch', function() {
  livereload.listen()
  gulp.watch('**/*.scss', ['sass']);
});

// Default task
gulp.task('default', ['sass']);

// Watch files for changes
gulp.task('devwatch', function() {
  livereload.listen()
  gulp.watch('**/*.scss', ['dev']);
});