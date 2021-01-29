Univ_Init{
	Dim i As Integer
	Dim One As Agt
	Dim Walkers As Agtset
	Dim Counter As Integer

	ClearConsoleScreen()
	println("初期感染者数: " & int(Universe.Initial_Infected) & " (" & round(10000*Universe.Initial_Infected / Universe.Population)/100 & "%)")

	//エージェント生成
	For i = 0 To Universe.Population - 1
		One = CreateAgt(Universe.Field.Walker)
		One.Direction = Rnd()*360
		One.RN = 0
		//グループ分けと行動範囲の設定
		if i / universe.Population < universe.group1_rate then
			one.action_radius = universe.action_radius_group1
		else
			one.action_radius = universe.action_radius_group2
		end if
	Next i
	MakeAgtset(Walkers, Universe.Field.Walker)
	RandomPutAgtset(Walkers)

	//感染状態の初期設定
	set_initial_infection(walkers)

	//色の設定
	adjust_color(walkers)
	

}

Univ_Step_Begin{

}

Univ_Step_End{
	Dim i As Integer
	Dim Walkers As Agtset
	Dim One As Agt
	Dim Pop As Double

	
	MakeAgtset(Walkers, Universe.Field.Walker)
	adjust_color(walkers)
	count_sir(walkers)

	//基本再生産数の平均計算
	Pop = 0
	Universe.AverageRN = 0
	For Each One in Walkers
		If  One.Counter > 1 Then
			Universe.AverageRN = Universe.AverageRN + One.RN
			Pop = Pop+1
		End if
	Next Walker
	If Pop <> 0		Then
		Universe.AverageRN = Universe.AverageRN/Pop
	Else
		Universe.AverageRN = 0
	End if
	
}

Univ_Finish{

}

//感染状態の初期設定
sub set_initial_infection(walkers as agtset){
Dim i As Integer
	Dim One As Agt
//	Dim Walkers As Agtset
	Dim Counter As Integer

	//初期の感染人数（確率）に応じた感染の初期設定
	If Universe.Initial_Infected < 1	Then
		//確率が設定されている場合、確率に応じた感染
		For Each One in Walkers
			If Rnd() < Universe.Initial_Infected	Then
				One.Condition = 1
				One.RN = 0
				universe.total_infected_num = universe.total_infected_num + 1
			Else
				One.Condition = 0
			End if
		Next One	
	Else
		//人数が設定されている場合、人数に応じた感染
		Counter = 0
		For Each One in Walkers
			If Counter < Universe.Initial_Infected	Then
				One.Condition = 1
				One.RN = 0
				Counter = Counter+1
				universe.total_infected_num = universe.total_infected_num + 1
			Else
				One.Condition = 0
			End if
		Next One
	End if

}

//色の設定
sub adjust_color(walkers as agtset){
dim one as agt

For Each One in Walkers
		//Color_Adjustment
		If 		One.Condition == 0	Then	//健常者（感受性者）
				One.Color = RGB(0, 200, 0)
		Elseif	One.Condition == 3	Then	//潜伏感染者	
				One.Color = RGB(200, 200, 100)
		Elseif	One.Condition == 1	Then	//感染者
				One.Color = RGB(200, 0, 0)
				if one.is_serious == True then
					one.color = RGB(153, 0, 0)  //重症者は濃い赤
				end if
		Elseif	One.Condition == 2	Then	//免疫保持者（除外者）	
				One.Color = RGB(0, 0, 200)
		elseif one.condition == 4 then  //死亡（追加）
			one.color = COLOR_BLACK
		End if
	Next One	
}

//状態のカウント
sub count_sir(walkers as agtset){
dim i as integer
dim one as agt

  //初期化
  For i = 0 to 3
		Universe.Rate_SIR(i) = 0
	Next i
	universe.serious_in_hospital = 0
	universe.serious_out_hospital = 0

	//数のカウント
	For Each One in Walkers	
		Universe.Rate_SIR(One.Condition) = Universe.Rate_SIR(One.Condition)+1
		//重症者に関するカウント
		if one.condition == 1 and one.is_serious == True then
			if one.in_hospital == True then
				universe.serious_in_hospital = universe.serious_in_hospital + 1
			else
				universe.serious_out_hospital = universe.serious_out_hospital + 1
			end if
		end if
	Next One

	Universe.total_infected_num = Universe.Population - Universe.Rate_SIR(0) - Universe.Rate_SIR(3)
	universe.total_infected_rate = universe.total_infected_num / universe.Population


	//比率の計算
	For i = 0 to 4
		Universe.Rate_SIR(i)=Universe.Rate_SIR(i)/CountAgtset(Walkers) 
	Next i
	

	//感染者も潜伏感染者もいなくなったら終了
	if Universe.rate_sir(3) == 0 and Universe.rate_sir(1) == 0 then
		println("累積感染者数: " & universe.total_infected_num & " (" & round(universe.total_infected_rate * 10000)/100 & "%)")
		println("死亡者数: " & int(Universe.Rate_SIR(4) * universe.Population) & " (" & round(Universe.Rate_SIR(4) * 10000)/100 & "%)")
		exitsimulation()
	end if
	
	//Rate_Sir(0)=健常者（感受性者）の比率
	//Rate_Sir(3)=潜伏感染者（潜伏感受性者）の比率
	//Rate_Sir(1)=感染者の比率
	//Rate_Sir(2)=免疫保持者（除外者）の比率
	//Rate_Sir(4)=死亡者の比率（追加）
}