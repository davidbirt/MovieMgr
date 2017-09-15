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
		fetchMovieq: function (companyId) {
			var deffered = $q.defer();
			$http.get(window.location.href + '/api/movies').then(function (response) {
				deffered.resolve(response);
			}, function (failed) {
				deffered.resolve(failed);
			});
			return deffered.promise;
		}
	}
}])
