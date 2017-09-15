angular.module('movies', ['ngRoute', function ($routeProvider) {
	$routeProvider.when('/', { templateUrl: '/www/views/movies-list.html' });
}]);