[기획 및 발표자료](https://github.com/GyeoungMin/DreamSurviver/blob/main/Dream%20Survivor.pdf)

초기 문제점은 많은량의 오브젝트때문에 풀을 적용하여도 FPS가 30프레임 이하로 떨어지며 렉이걸리는것으로 확인이 되었다.
때문에 해당 문제를 해결하기 위해서 QuadTree적용하고 Culling기법과 풀을 같이 사용함으로써 평균 60프레임이 되는것을 확인하였다.
