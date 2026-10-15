(Program
	(ProgramHeading
		Program 'program'
		(Identifier
			Letter 'L'
			Letter 'a'
			Letter 'b'
			Letter 'e'
			Letter 'l'
			Letter 's'
		)
		OpenParen '('
		(ProgramParameterList
			(IdentifierList
				(Identifier -> variable output: text
					Letter 'o'
					Letter 'u'
					Letter 't'
					Letter 'p'
					Letter 'u'
					Letter 't'
				)
			)
		)
		CloseParen ')'
	)
	Semicolon ';'
	(ProgramBlock
		(Block
			(LabelDeclarationPart
				Label 'label'
				(Label -> label 10
					(DigitSequence
						Digit '1'
						Digit '0'
					)
				)
				Comma ','
				(Label -> label 20
					(DigitSequence
						Digit '2'
						Digit '0'
					)
				)
				Comma ','
				(Label -> label 99
					(DigitSequence
						Digit '9'
						Digit '9'
					)
				)
				Semicolon ';'
			)
			(ConstantDefinitionPart)
			(TypeDefinitionPart)
			(VariableDeclarationPart
				Var 'var'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable i: integer
							Letter 'i'
						)
					)
					Colon ':'
					(TypeDenoter : integer
						(TypeIdentifier : integer
							(Identifier -> type integer
								Letter 'i'
								Letter 'n'
								Letter 't'
								Letter 'e'
								Letter 'g'
								Letter 'e'
								Letter 'r'
							)
						)
					)
				)
				Semicolon ';'
			)
			(ProcedureAndFunctionDeclarationPart
				(ProcedureDeclaration
					(ProcedureHeading
						Procedure 'procedure'
						(Identifier -> procedure escape
							Letter 'e'
							Letter 's'
							Letter 'c'
							Letter 'a'
							Letter 'p'
							Letter 'e'
						)
					)
					Semicolon ';'
					(ProcedureBlock
						(Block
							(LabelDeclarationPart
								Label 'label'
								(Label -> label 5
									(DigitSequence
										Digit '5'
									)
								)
								Semicolon ';'
							)
							(ConstantDefinitionPart)
							(TypeDefinitionPart)
							(VariableDeclarationPart)
							(ProcedureAndFunctionDeclarationPart)
							(StatementPart
								(CompoundStatement
									Begin 'begin'
									(StatementSequence
										(Statement
											(SimpleStatement
												(AssignmentStatement
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> variable i: integer
																	Letter 'i'
																)
															)
														)
													)
													Walrus ':='
													(Expression : integer
														(SimpleExpression : integer
															(Term : integer
																(Factor : integer
																	(UnsignedConstant : integer
																		(UnsignedNumber : integer
																			(UnsignedInteger : integer
																				(DigitSequence
																					Digit '0'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Semicolon ';'
										(Statement
											(Label -> label 5
												(DigitSequence
													Digit '5'
												)
											)
											Colon ':'
											(SimpleStatement
												(AssignmentStatement
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> variable i: integer
																	Letter 'i'
																)
															)
														)
													)
													Walrus ':='
													(Expression : integer
														(SimpleExpression : integer
															(Term : integer
																(Factor : integer
																	(VariableAccess : integer
																		(EntireVariable : integer
																			(VariableIdentifier : integer
																				(Identifier -> variable i: integer
																					Letter 'i'
																				)
																			)
																		)
																	)
																)
															)
															(AddingOperator
																Plus '+'
															)
															(Term : integer
																(Factor : integer
																	(UnsignedConstant : integer
																		(UnsignedNumber : integer
																			(UnsignedInteger : integer
																				(DigitSequence
																					Digit '1'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Semicolon ';'
										(Statement
											(StructuredStatement
												(ConditionalStatement
													(IfStatement
														If 'if'
														(BooleanExpression : Boolean
															(Expression : Boolean
																(SimpleExpression : integer
																	(Term : integer
																		(Factor : integer
																			(VariableAccess : integer
																				(EntireVariable : integer
																					(VariableIdentifier : integer
																						(Identifier -> variable i: integer
																							Letter 'i'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(RelationalOperator
																	LessThan '<'
																)
																(SimpleExpression : integer
																	(Term : integer
																		(Factor : integer
																			(UnsignedConstant : integer
																				(UnsignedNumber : integer
																					(UnsignedInteger : integer
																						(DigitSequence
																							Digit '3'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														Then 'then'
														(Statement
															(SimpleStatement
																(GotoStatement
																	Goto 'goto'
																	(Label -> label 5
																		(DigitSequence
																			Digit '5'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Semicolon ';'
										(Statement
											(StructuredStatement
												(ConditionalStatement
													(IfStatement
														If 'if'
														(BooleanExpression : Boolean
															(Expression : Boolean
																(SimpleExpression : integer
																	(Term : integer
																		(Factor : integer
																			(VariableAccess : integer
																				(EntireVariable : integer
																					(VariableIdentifier : integer
																						(Identifier -> variable i: integer
																							Letter 'i'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(RelationalOperator
																	Equals '='
																)
																(SimpleExpression : integer
																	(Term : integer
																		(Factor : integer
																			(UnsignedConstant : integer
																				(UnsignedNumber : integer
																					(UnsignedInteger : integer
																						(DigitSequence
																							Digit '3'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														Then 'then'
														(Statement
															(SimpleStatement
																(GotoStatement
																	Goto 'goto'
																	(Label -> label 99
																		(DigitSequence
																			Digit '9'
																			Digit '9'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
									)
									End 'end'
								)
							)
						)
					)
				)
				Semicolon ';'
			)
			(StatementPart
				(CompoundStatement
					Begin 'begin'
					(StatementSequence
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : integer
										(EntireVariable : integer
											(VariableIdentifier : integer
												(Identifier -> variable i: integer
													Letter 'i'
												)
											)
										)
									)
									Walrus ':='
									(Expression : integer
										(SimpleExpression : integer
											(Term : integer
												(Factor : integer
													(UnsignedConstant : integer
														(UnsignedNumber : integer
															(UnsignedInteger : integer
																(DigitSequence
																	Digit '0'
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(Label -> label 10
								(DigitSequence
									Digit '1'
									Digit '0'
								)
							)
							Colon ':'
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : integer
										(EntireVariable : integer
											(VariableIdentifier : integer
												(Identifier -> variable i: integer
													Letter 'i'
												)
											)
										)
									)
									Walrus ':='
									(Expression : integer
										(SimpleExpression : integer
											(Term : integer
												(Factor : integer
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> variable i: integer
																	Letter 'i'
																)
															)
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : integer
												(Factor : integer
													(UnsignedConstant : integer
														(UnsignedNumber : integer
															(UnsignedInteger : integer
																(DigitSequence
																	Digit '1'
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(ConditionalStatement
									(IfStatement
										If 'if'
										(BooleanExpression : Boolean
											(Expression : Boolean
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(VariableAccess : integer
																(EntireVariable : integer
																	(VariableIdentifier : integer
																		(Identifier -> variable i: integer
																			Letter 'i'
																		)
																	)
																)
															)
														)
													)
												)
												(RelationalOperator
													LessThan '<'
												)
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(UnsignedNumber : integer
																	(UnsignedInteger : integer
																		(DigitSequence
																			Digit '5'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Then 'then'
										(Statement
											(SimpleStatement
												(GotoStatement
													Goto 'goto'
													(Label -> label 10
														(DigitSequence
															Digit '1'
															Digit '0'
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(Label -> label 20
								(DigitSequence
									Digit '2'
									Digit '0'
								)
							)
							Colon ':'
							(StructuredStatement
								(CompoundStatement
									Begin 'begin'
									(StatementSequence
										(Statement
											(StructuredStatement
												(ConditionalStatement
													(IfStatement
														If 'if'
														(BooleanExpression : Boolean
															(Expression : Boolean
																(SimpleExpression : integer
																	(Term : integer
																		(Factor : integer
																			(VariableAccess : integer
																				(EntireVariable : integer
																					(VariableIdentifier : integer
																						(Identifier -> variable i: integer
																							Letter 'i'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(RelationalOperator
																	GreaterThan '>'
																)
																(SimpleExpression : integer
																	(Term : integer
																		(Factor : integer
																			(UnsignedConstant : integer
																				(UnsignedNumber : integer
																					(UnsignedInteger : integer
																						(DigitSequence
																							Digit '1'
																							Digit '0'
																							Digit '0'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														Then 'then'
														(Statement
															(SimpleStatement
																(GotoStatement
																	Goto 'goto'
																	(Label -> label 20
																		(DigitSequence
																			Digit '2'
																			Digit '0'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
									)
									End 'end'
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure escape
											Letter 'e'
											Letter 's'
											Letter 'c'
											Letter 'a'
											Letter 'p'
											Letter 'e'
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(Label -> label 99
								(DigitSequence
									Digit '9'
									Digit '9'
								)
							)
							Colon ':'
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure writeln
											Letter 'w'
											Letter 'r'
											Letter 'i'
											Letter 't'
											Letter 'e'
											Letter 'l'
											Letter 'n'
										)
									)
									(WritelnParameterList
										OpenParen '('
										(WriteParameter
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(VariableAccess : integer
																(EntireVariable : integer
																	(VariableIdentifier : integer
																		(Identifier -> variable i: integer
																			Letter 'i'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
					)
					End 'end'
				)
			)
		)
	)
	Dot '.'
)