angular.module('movies', ['ngRoute', function ($routeProvider) {
	$routeProvider.when('/', { templateUrl: '/www/views/movies-list.html' });
	$routeProvider.when("/edit/:movieId", { templateUrl: "www/views/movies-edit.html" })
}]);