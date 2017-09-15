angular.module('movies').service('movieSrv', ['$http', '$q', function ($http, $q) {
	var movieData = {
		list:[] 
	};
	function setMovieData(data) {
		movieData['list'] = data;
	}
	return {
		movies: movieData,
		setMovieData: setMovieData,
		fetchMovies: function () {
			var deffered = $q.defer();
			$http.get(window.location.origin + '/api/movies').then(function (response) {
				deffered.resolve(response);
			}, function (failed) {
				deffered.resolve(failed);
			});
			return deffered.promise;
		},
		fetchMovie: function (id) {
			var deffered = $q.defer();
			$http.get(window.location.origin + '/api/movies/' + id).then(function (response) {
				deffered.resolve(response);
			}, function (failed) {
				deffered.resolve(failed);
			});
			return deffered.promise;
		},
		saveMovie: function (movie) {
			var deffered = $q.defer();
			$http.post(window.location.origin + '/api/movies', movie).then(function (response) {
				deffered.resolve(response);
			}, function (failed) {
				deffered.resolve(failed);
			});
			return deffered.promise;
		}
	}
}])
