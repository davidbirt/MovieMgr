angular.module('movies', ['ngRoute', 'angularUtils.directives.dirPagination', function ($routeProvider) {
	$routeProvider.when('/', { templateUrl: '/www/views/movies-list.html' });
	$routeProvider.when("/edit/:movieId", { templateUrl: "www/views/movies-edit.html" })
}]);