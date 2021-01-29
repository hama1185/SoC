Agt_Init{

}

Agt_Step{
	Dim i As Integer	
	Dim Neighbors As Agtset
	Dim One As Agt
	Dim Trigger As Integer
	Dim OptionCandidate As Integer
	Dim center_x As double
	Dim center_y As double
	Dim all_agts as agtset
	
	//行動範囲内の人を集める
	MakeAllAgtsetAroundOwn(all_agts, my.action_radius, False)

	//行動範囲内の人からランダムに接触人数の最大値まで接触
	for i = 0 to Universe.max_contact - 1
		if countagtset(all_agts) == 0 then
			break  //全員に接触したら終わり
		end if
		one = getagt(all_agts, int(rnd()*countagtset(all_agts)))  //ランダムに一人選ぶ
		addagt(neighbors, one)  //接触した人に追加
		removeagt(all_agts, one)  
	next i 
		
	//行動半径の円を描く
	//center_x = my.X - Universe.action_radius/2
	//center_y = my.Y - Universe.action_radius/2
	//DrawArc("Field", COLOR_BLACK, 0, center_x, center_y, Universe.action_radius, Universe.action_radius, 0, 360, False)

	//接触した人に感染させる
	infection_behavior(neighbors)


}

sub infection_behavior(neighbors as agtset) {
	Dim i As Integer	
//	Dim Neighbors As Agtset
	Dim One As Agt
	Dim Trigger As Integer
	Dim OptionCandidate As Integer
	
	

	//SIRモデルのとき
	If Universe.Scenario == "SIR"		Then
		If 		My.Condition == 0	Then	//健康なとき→なにもしない

		Elseif	My.Condition == 1	Then	//感染性をもったとき
			My.Counter = My.Counter+1	//感染期間を数える
			
			For Each One in Neighbors	//感染させる
				If One.Condition == 0 and Rnd() < Universe.Infection_Rate Then
//					One.Condition = 1
					develop(one)  //発症させる
					One.RN = 0
					My.RN = My.RN + 1
				End if
			Next One
			
			If	1/Universe.Infection_Period > Rnd()	Then	//回復する
//					My.Condition = 2
					recover_or_die(my)  //回復または死亡
					My.Counter = 0
			End if
		
		Elseif	My.Condition == 2	Then	//免疫をもったとき→なにもしない

		End if

	//SISモデルのとき
//	Elseif	Universe.Scenario == "SIS"		Then
//		If 		My.Condition == 0	Then	//健康なとき
//
//		Elseif	My.Condition == 1	Then	//感染性をもったとき
//			My.Counter = My.Counter+1	//感染期間を数える
//			
//			For Each One in Neighbors	//感染させる
//				If One.Condition == 0 and Rnd() < Universe.Infection_Rate Then
////					One.Condition = 1
//					develop(one)  //発症させる
//					One.RN = 0
//					My.RN = My.RN + 1
//				End if
//			Next One
//			
//			If	1/Universe.Infection_Period > Rnd()	Then	//回復する
//					My.Condition = 0  //ToDo: このとき行動半径をリセットする必要あり
////						recover_or_die(my)  //回復または死亡
//					My.Counter = 0
//			End if
//		Elseif	My.Condition == 2	Then	//免疫をもったとき
//
//		End if
//
//	//SIモデルのとき
//	Elseif	Universe.Scenario == "SI"		Then
//		If 		My.Condition == 0	Then	//健康なとき
//
//		Elseif	My.Condition == 1	Then	//感染性をもったとき
//			My.Counter = My.Counter+1	//感染期間を数える
//			
//			For Each One in Neighbors	//感染させる
//				If One.Condition == 0 and Rnd() < Universe.Infection_Rate Then
////					One.Condition = 1
//					develop(one)  //発症させる
//					One.RN = 0
//					My.RN = My.RN + 1
//				End if
//			Next One
//		
//		Elseif	My.Condition == 2	Then	//免疫をもったとき
//
//		End if
//
//	//SEIR_01モデルのとき
//	Elseif	Universe.Scenario == "SEIR_01"		Then
//		If 		My.Condition == 0	Then	//健康なとき
//
//		Elseif	My.Condition == 1	Then	//感染したとき
//			My.Counter = My.Counter+1	//感染期間を数える
//			
//			For Each One in Neighbors	//感染させる
//				If One.Condition == 0 and Rnd() < Universe.Infection_Rate Then
//					If Universe.Incubation_Period <> 0	Then	//潜伏期間が0でないときは潜伏感染者になる
//						One.Condition = 3
//						One.RN = 0
//						My.RN = My.RN + 1
//					Else										//潜伏期間が0のときはすぐに感染者にする
////						One.Condittion = 1
//						develop(one)  //発症させる
//						One.RN = 0
//						One.Counter = 0					
//					End if
//				End if
//			Next One
//			
//			If	1/Universe.Infection_Period > Rnd()	Then	//回復する
////					My.Condition = 2
//					recover_or_die(my)  //回復または死亡
//					My.Counter = 0
//			End if
//			
//		Elseif	My.Condition == 3	Then	//潜伏感染者のとき（感染性のない感染者）
//			My.Counter = My.Counter+1	//感染期間を数える
//			If	1/Universe.Incubation_Period > Rnd()	Then	//発症する
////					My.Condition = 1
//					develop(my)  //自分が発症する
//					My.Counter = 0
//			End if
//			If	1/Universe.Infection_Period > Rnd()	Then	//回復する
////					My.Condition = 2
//					recover_or_die(my)  //回復または死亡
//					My.Counter = 0
//			End if
//		End if
	
	//SEIR_02モデルのとき
	Elseif	Universe.Scenario == "SEIR_02"		Then
		If 		My.Condition == 0	Then	//健康なとき

		Elseif	My.Condition == 1	Then	//感染したとき
			My.Counter = My.Counter+1	//感染期間を数える
			
			For Each One in Neighbors	//感染させる
				If One.action_radius <> Universe.action_radius_group3 Then
					If One.Condition == 0 and Rnd() < Universe.Infection_Rate Then
						If Universe.Incubation_Period <> 0	Then	//潜伏期間が0でないときは潜伏感染者になる
							One.Condition = 3
							One.RN = 0			
							My.RN = My.RN + 1
						Else										//潜伏期間が0のときはすぐに感染者にする
	//						One.Condittion = 1
							develop(one)  //発症する
							One.RN = 0
							One.Counter = 0
							My.RN = My.RN + 1
						End if
					End if
				Else //グループCの時
					If One.Condition == 0 and Rnd() < Universe.Infection_Nonemask_Rate Then
						If Universe.Incubation_Period <> 0	Then	//潜伏期間が0でないときは潜伏感染者になる
							One.Condition = 3
							One.RN = 0			
							My.RN = My.RN + 1
						Else										//潜伏期間が0のときはすぐに感染者にする
	//						One.Condittion = 1
							develop(one)  //発症する
							One.RN = 0
							One.Counter = 0
							My.RN = My.RN + 1
						End if
					End if
				End if
			Next One
			
			If	1/Universe.Infection_Period > Rnd()	Then	//回復する
//					My.Condition = 2
					recover_or_die(my)  //回復または死亡
					My.Counter = 0
			End if
		Elseif	My.Condition == 3	Then	//潜伏感染者のとき（感染性のある感染者）
			My.Counter = My.Counter+1	//感染期間を数える
			For Each One in Neighbors	//感染させる
				If One.action_radius <> Universe.action_radius_group3 Then
					If One.Condition == 0 and Rnd() < Universe.Infection_Rate Then
						One.Condition = 3
						One.RN = 0
						My.RN = My.RN + 1
					End if
				Else
					If One.Condition == 0 and Rnd() < Universe.Infection_Nonemask_Rate Then
						One.Condition = 3
						One.RN = 0
						My.RN = My.RN + 1
					End if
				End if
			Next One
			If	1/Universe.Incubation_Period > Rnd()	Then	//発症する
//					My.Condition = 1
					develop(my)  //自分が発症する
					My.Counter = 0
			End if
			If	1/Universe.Infection_Period > Rnd()	Then	//回復する
//					My.Condition = 2
					recover_or_die(my)  //回復または死亡
					My.Counter = 0
			End if
		End if	

//	//SIRSモデルのとき
//	Elseif	Universe.Scenario == "SIRS"		Then
//		If 		My.Condition == 0	Then	//健康なとき
//
//		Elseif	My.Condition == 1	Then	//感染性をもったとき
//			My.Counter = My.Counter+1	//感染期間を数える
//			
//			For Each One in Neighbors	//感染させる
//				If One.Condition == 0 and Rnd() < Universe.Infection_Rate Then
////					One.Condition = 1
//					develop(one)  //感染させる
//					One.RN = 0
//					My.RN = My.RN + 1
//				End if
//			Next One
//			
//			If	1/Universe.Infection_Period > Rnd()	Then	//回復する
//			If	Universe.Immune_Period <> 0	Then		//免疫期間が0の設定のときは免疫を獲得しない
////					My.Condition = 2
//					recover_or_die(my)  //回復または死亡
//					My.Counter = 0
//			End if
//			End if
//		Elseif	My.Condition == 2	Then	//免疫をもったとき
//			If	1/Universe.Immune_Period > Rnd()	Then	//免疫がなくなる
//					My.Condition = 0
//					My.Counter = 0
//			End if
//		End if	
	End if		
	
}

//発症させる
sub develop(one as agt){

  one.condition = 1  //発症
  one.action_radius = universe.action_radius_infected  //発症したら行動半径を変える
  
  //合計感染者数カウント
//	Universe.total_infected_num = Universe.total_infected_num + 1

  //確率に応じて重症化
  if rnd() < universe.serious_rate then
  	one.is_serious = True
  	universe.serious_num = universe.serious_num + 1
  end if

  //設定に応じて病院に収容（しようとする）
  if universe.only_serious_in_hospital == True then
  	if one.is_serious == True then
  		try_hospitalize(one)
 		end if
 	else
 		try_hospitalize(one)
 	end if
}

//病院に収容しようとする
sub try_hospitalize(one as agt){

	if universe.in_hospital_num <= universe.hospital_capacity then
		//病院収容可能
		one.in_hospital = True
		universe.in_hospital_num = universe.in_hospital_num + 1
		one.death_rate = universe.death_rate_in_hospital
	else
		one.death_rate = universe.death_rate_out_hospital
	end if
}

//重症時は確率に従って死亡
sub recover_or_die(one as agt){

	//いずれにせよ退院
	if one.in_hospital == True then
		universe.in_hospital_num = universe.in_hospital_num - 1
	end if
	
	if one.is_serious == False then
		//軽症時は回復
		one.condition = 2
	else
		//重症時は死亡率に従って死亡
		universe.serious_num = universe.serious_num - 1
		if rnd() < one.death_rate then
			one.condition = 4  //死亡
		else
			one.condition = 2  //回復
		end if		
	end if

}