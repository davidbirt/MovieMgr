angular.module('movies').controller('mainCtrl', ['$scope','movieSrv' ,function ($scope, movies) {
	MC = $scope;
	$scope.message = 'chello';
}]);
