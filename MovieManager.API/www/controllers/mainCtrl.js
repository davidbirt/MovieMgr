angular.module('movies').controller('mainCtrl', ['$scope', 'movieSrv', '$location' ,function ($scope, movies,$location) {
	MC = $scope;
	$scope.message = 'chello';
	$scope.moviesList = movies.movies;
	// if there are not any movies loaded up then go get them.
	if (movies.movies['list'].length == 0)
	{
		movies.fetchMovies().then(function (response) {
			movies.movies['list'] = response.data;
			console.log(response);
		})
	}

	$scope.log = function (id) {
		$location.path('/edit/' + id);
	}

}]);
