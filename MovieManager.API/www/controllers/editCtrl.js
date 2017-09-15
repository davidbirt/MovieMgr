angular.module('movies').controller('editCtrl', ['$scope', 'movieSrv', '$routeParams', function ($scope, movies, routes) {
	EC = $scope;
	$scope.message = 'editing';
	// load up the movie from its route.
	movies.fetchMovie(routes.movieId).then(function (response) {
		$scope.movie = response.data;
	});
}]);
